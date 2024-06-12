using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
	public class StudentCreateModelDto
	{

		public required string FirstName { get; set; }

		public required string LastName { get; set; }

		public required string Email { get; set; }

		public required string Password { get; set; }

		public required int Semester { get; set; }
		public required int Wiek { get; set; }

		
	}

}