using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities
{
	public class Subject
	{
		public Guid Id { get; set; }
		public required string SubjectName { get; set; }
		public required string SubjectDescription { get; set; }
		public ICollection<StudentCourse> StudentCourses { get; set; }

	}
}