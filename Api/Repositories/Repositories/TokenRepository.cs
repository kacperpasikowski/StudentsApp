using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Repositories.Repositories
{
	public class TokenRepository : ITokenRepository
	{
		private readonly SymmetricSecurityKey _key;

		private readonly UserManager<ApplicationUser> _userManager;
		
		public TokenRepository(IConfiguration config, UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;

			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
			
		}
		public async Task<string> CreateJwtToken(ApplicationUser user)
		{
			var claims = new List<Claim> 
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.Id),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
			};
			
			var roles = await _userManager.GetRolesAsync(user);
			
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
			
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
			
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds
			};
			
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}