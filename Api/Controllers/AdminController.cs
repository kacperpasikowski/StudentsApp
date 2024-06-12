using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AdminController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly DataContext _context;
		private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
		private readonly AuthDataContext _authDataContext;

		public AdminController(UserManager<ApplicationUser> userManager, DataContext context,
				 IPasswordHasher<ApplicationUser> passwordHasher, AuthDataContext authDataContext)
		{
			_authDataContext = authDataContext;
			_passwordHasher = passwordHasher;
			_userManager = userManager;
			_context = context;
		}

		[HttpPost("addstudent")]
		public async Task<ActionResult> AddStudent([FromBody] StudentCreateModelDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var student = new Student
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				Wiek = model.Wiek,
				Semester = model.Semester,
				Avatar = GetDefaultAvatar()

			};

			var user = new ApplicationUser
			{
				UserName = GenerateUsername(model.FirstName, model.LastName, student.Id),
				Email = model.Email.Trim(),
				AvatarUrl = GetDefaultAvatar()

			};

			var passwordHash = _passwordHasher.HashPassword(user, model.Password);
			user.PasswordHash = passwordHash;

			var result = await _userManager.CreateAsync(user);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			await _userManager.AddToRoleAsync(user, "Student");




			_context.Students.Add(student);

			await _context.SaveChangesAsync();

			return Ok(student);

		}
		[HttpGet]
		[Route("{userName}")]
		public async Task<ActionResult> GetUserByUsername(string userName)
		{
			var currentUserName = await _authDataContext.Users.FindAsync(userName);

			if (currentUserName == null)
			{
				return NotFound();
			}
			return Ok(currentUserName);
		}

		[HttpGet]
		[Route("users-with-roles")]
		public async Task<ActionResult<PagedList<UserWithRolesDTO>>> GetAllUsersWithRolesAsync([FromQuery] UserParams userParams)
		{
			var query = _userManager.Users
				.OrderBy(u => u.UserName)
				.Select(u => new UserWithRolesDTO
				{
					Id = u.Id,
					Username = u.UserName,
					Email = u.Email,
					Roles = _userManager.GetRolesAsync(u).Result
				}).AsQueryable();

			var pagedList = await PagedList<UserWithRolesDTO>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);

			Response.AddPaginationHeader(new PaginationHeader(
				pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount, pagedList.TotalPages));

			return Ok(pagedList);
		}

		[HttpPost("edit-roles/{username}")]
		public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
		{
			if (string.IsNullOrEmpty(roles)) return BadRequest("you dont have a role, whatcha doin ehre");

			var selectedRoles = roles.Split(",").ToArray();

			var user = await _userManager.FindByNameAsync(username);
			if (user == null) return NotFound();

			var userRoles = await _userManager.GetRolesAsync(user);

			var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

			if (!result.Succeeded) return BadRequest("Failed to add to roles");

			result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

			if (!result.Succeeded) return BadRequest("Failed to remove from roles");

			return Ok(await _userManager.GetRolesAsync(user));

		}

		private string GenerateUsername(string firstName, string lastName, Guid studentId)
		{
			Random rand = new Random();
			string username = $"{firstName.Substring(0, 5)}{lastName.Substring(0, 5)}{lastName.Substring(0, 4)}";
			// Dodaj dodatkową logikę, jeśli chcesz uniknąć powtórzeń lub inne transformacje nazwy użytkownika
			return username;
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