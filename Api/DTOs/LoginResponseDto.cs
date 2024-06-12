using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
	public class LoginResponseDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
		public List<string>  Roles { get; set; }
	}
}