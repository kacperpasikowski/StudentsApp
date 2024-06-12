using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Api.Entities
{
	public class ApplicationUser : IdentityUser
	{
		
		public byte[] AvatarUrl { get; set; }
	}
}