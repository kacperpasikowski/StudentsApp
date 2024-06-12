using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
    public class RegisterResponseDto
    {
        public string Email { get; set; }
		public List<string>  Roles { get; set; }
    }
}