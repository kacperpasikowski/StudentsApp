using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities
{
	public class Grade
	{
		public Guid Id { get; set; }
		public Guid StudentCourseId { get; set; }
		public required StudentCourse StudentCourse { get; set; }
		public int ExerciseGrade { get; set; }
		public int LectureGrade { get; set; }
		public int FinalGrade { get; set; }
	}
}