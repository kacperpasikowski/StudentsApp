using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities
{
	public class Student
	{
		public Guid Id { get; set; }
		public required string FirstName { get; set; }
		public required string LastName { get; set; }
		public string Email { get; set; }
		public int Semester { get; set; }
		public ICollection<StudentCourse> StudentCourses { get; set; }

		public byte[] Avatar { get; set; }
		public int Wiek { get; set; }
	}
}