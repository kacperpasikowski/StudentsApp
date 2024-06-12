using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities
{
	public class StudentCourse
	{
		public Guid Id { get; set; }
		public Guid StudentId { get; set; }
		public  Student Student { get; set; }
		public Guid SubjectId { get; set; }
		public  Subject Subject { get; set; }
		public DateOnly StartingDate { get; set; }
		public ICollection<Grade> Grades { get; set; } = null;
	}
}