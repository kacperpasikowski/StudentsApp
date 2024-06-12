using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTOs
{
	public class GradeDto
	{
		[Required]
		public int ExerciseGrade { get; set; }

		[Required]
		public int LectureGrade { get; set; }
		[Required]
		public int FinalGrade { get; set; }


	}

	
}