using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
	public class LoginRequestDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}