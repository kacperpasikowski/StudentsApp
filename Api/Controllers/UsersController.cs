using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		
		public UsersController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}
		
		
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
		{
			return await _userManager.Users.ToListAsync();
		}
		
		
		[HttpGet("{userEmail}/photo")]
		public async Task<ActionResult> GetStudentPhoto(string userEmail)
		{
			var user = await _userManager.FindByEmailAsync(userEmail);
			
			if(user == null)
			{
				return NotFound("Nie znaleziono użytkownika o podanym Id"); 
			}
			
			return File(user.AvatarUrl, "image/png");
		}
		
		
		
		
		
		
		
	}
}