using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities;

namespace Api.DTOs
{
	public class StudentCourseDto
	{
		public Guid Id { get; set; }
		public Guid StudentId { get; set; }
		public string StudentFirstName { get; set; }
		public string StudentLastName { get; set; }
		public Guid SubjectId { get; set; }
		public string SubjectName { get; set; } 
		public DateOnly StartingDate { get; set; }
		
		public StudentCourseDto(StudentCourse studentCourse)
    {
        Id = studentCourse.Id;
        StudentId = studentCourse.StudentId;
        StudentFirstName = studentCourse.Student?.FirstName; 
        StudentLastName = studentCourse.Student?.LastName;  
        SubjectId = studentCourse.SubjectId;
        SubjectName = studentCourse.Subject?.SubjectName; 
        StartingDate = studentCourse.StartingDate;
    }
	}
}