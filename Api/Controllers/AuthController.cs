using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenRepository _tokenRepository;
		private readonly AuthDataContext _context;
		public AuthController(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository,
		 AuthDataContext context)
		{
			_tokenRepository = tokenRepository;
			_userManager = userManager;
			_context = context;
		}
		// POST: {baseurl}/api/auth/register
		[HttpPost]
		[Route("register")]
		public async Task<ActionResult> Register([FromBody] RegisterRequestDto request)
		{
			var user = new ApplicationUser
			{
				UserName = request.UserName.Trim(),
				Email = request.Email.Trim(),
				AvatarUrl = GetDefaultAvatar()
			};

			var identityResult = await _userManager.CreateAsync(user, request.Password);

			if (identityResult.Succeeded)
			{
				identityResult = await _userManager.AddToRoleAsync(user, "Viewer");

				var roles = await _userManager.GetRolesAsync(user);

				var jwtToken = await _tokenRepository.CreateJwtToken(user);

				var response = new LoginResponseDto()
				{
					Email = request.Email,
					Roles = roles.ToList(),
					Token = jwtToken
				};

				if (identityResult.Succeeded)
				{
					return Ok(response);
				}
				else
				{
					if (identityResult.Errors.Any())
					{
						foreach (var error in identityResult.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
			}
			else
			{
				if (identityResult.Errors.Any())
				{
					foreach (var error in identityResult.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			return ValidationProblem(ModelState);
		}

		//https://localhost5001/auth/login
		[HttpPost]
		[Route("login")]
		public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
		{
			var identityUser = await _userManager.Users
												 .Where(u => u.Email == request.Email)
												 .SingleOrDefaultAsync();

			if (identityUser != null)
			{
				var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == identityUser.Id);

				if (applicationUser != null)
				{
					var jwtToken = await _tokenRepository.CreateJwtToken(applicationUser);

					var roles = await _userManager.GetRolesAsync(identityUser);

					var response = new LoginResponseDto()
					{
						Username = applicationUser.UserName,
						Email = request.Email,
						Roles = roles.ToList(),
						Token = jwtToken
					};

					return Ok(response);
				}
			}

			ModelState.AddModelError("", "Email or Password Incorrect");

			return ValidationProblem(ModelState);
		}

		private byte[] GetDefaultAvatar()
		{
			string defaultAvatarPath = "./Assets/ziomek.png";


			using (Image<Rgba32> image = Image.Load<Rgba32>(defaultAvatarPath))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					image.SaveAsPng(memoryStream);
					return memoryStream.ToArray();
				}
			}
		}
	}
}