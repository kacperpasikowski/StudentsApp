using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StudentController : ControllerBase
	{
		private readonly DataContext _context;

		public StudentController(DataContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<Student>>> GetStudentsAsync([FromQuery] UserParams userParams)
		{
			var query = _context.Students.AsNoTracking();

			switch (userParams.SortBy)
			{
				case "wiek":
					query = (userParams.SortOrder == "asc") ? query.OrderBy(s => s.Wiek) : query.OrderByDescending(s => s.Wiek);
					break;
				case "semester":
					query = (userParams.SortOrder == "asc") ? query.OrderBy(s => s.Semester) : query.OrderByDescending(s => s.Semester);
					break;
				default:
					query = query.OrderBy(s => s.LastName);
					break;
			}


			var pagedList = await PagedList<Student>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);

			Response.AddPaginationHeader(new PaginationHeader(
				pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount, pagedList.TotalPages));

			return Ok(pagedList);
		}
		[HttpGet("{email}")]
		public async Task<ActionResult<Student>> GetStudentByEmailAsync(string email)
		{
			var student = await _context.Students.FirstOrDefaultAsync(x => x.Email == email);

			if (student == null)
			{
				return NotFound("Nie znaleziono studenta o podanym Id");
			}
			if (student.Avatar == null)
			{
				return NotFound("Nie znaleziono awataru");
			}

			return Ok(student);
		}
		[Authorize(Roles = "Admin")]
		[HttpPut]
		[Route("{guid}")]
		public async Task<ActionResult> UpdateStudentInfo(Guid guid, Student updatedStudentInfo)
		{
			var studentToUpdate = await _context.Students.FindAsync(guid);

			if (studentToUpdate == null)
			{
				return NotFound("Nie znaleziono studenta");
			}

			studentToUpdate.FirstName = updatedStudentInfo.FirstName;
			studentToUpdate.LastName = updatedStudentInfo.LastName;
			studentToUpdate.Email = updatedStudentInfo.Email;
			studentToUpdate.Semester = updatedStudentInfo.Semester;
			studentToUpdate.Wiek = updatedStudentInfo.Wiek;

			_context.Students.Update(studentToUpdate);
			await _context.SaveChangesAsync();

			return Ok();

		}
		[HttpGet("{studentId}/photo")]
		public async Task<ActionResult> GetStudentPhoto(Guid studentId)
		{
			var student = await _context.Students.FindAsync(studentId);

			if (student == null)
			{
				return NotFound("Nie znaleziono studenta o podanym Id");
			}

			return File(student.Avatar, "image/png");
		}

		[HttpDelete]
		[Route("{guid}")]
		public async Task<ActionResult> DeleteStudentAsync(Guid guid)
		{
			var studentToRemove = await _context.Students.FindAsync(guid);

			if (studentToRemove == null)
			{
				return NotFound("Nie znaleziono studenta o podanym Id");
			}

			_context.Students.Remove(studentToRemove);
			await _context.SaveChangesAsync();
			return Ok();
		}
		[HttpGet("export")]
		public ActionResult ExportStudentsToCsv()
		{
			try
			{
				var students = _context.Students.ToList();
				var csv = GenerateCsv(students);

				var bytes = Encoding.UTF8.GetBytes(csv);
				var output = new FileContentResult(bytes, "text/csv")
				{
					FileDownloadName = "students.csv"
				};
				return output;
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occured while generating the CSV file.");
			}
		}
		[HttpGet("{email}/courses")]
		public async Task<ActionResult<IEnumerable<StudentCourseDto>>> GetStudentCourses(string email)
		{
			var student = await _context.Students
				.Include(s => s.StudentCourses)
				.ThenInclude(sc => sc.Subject)
				.FirstOrDefaultAsync(s => s.Email == email);

			if (student == null)
				return NotFound($"Student o mailu '{email}' nie istnieje.");

			var studentCourses = student.StudentCourses.Select(sc => new StudentCourseDto(sc)).ToList();

			return Ok(studentCourses);


		}


		[HttpPost("enroll")]
		public async Task<ActionResult<StudentCourseDto>> EnrollStudentInSubject([FromBody] EnrollmentRequestDto request)
		{
			var student = await _context.Students
				.Include(s => s.StudentCourses)
				.FirstOrDefaultAsync(s => s.Email == request.StudentEmail);

			var subject = await _context.Subjects
				.FirstOrDefaultAsync(s => s.SubjectName == request.SubjectName);

			// Sprawdzenie, czy student i przedmiot istnieją
			if (student == null)
				return NotFound($"Student o mailu '{request.StudentEmail}' nie istnieje.");

			if (subject == null)
				return NotFound($"Przedmiot o nazwie '{request.SubjectName}' nie istnieje.");

			// Sprawdzenie, czy student jest już zapisany na przedmiot
			if (student.StudentCourses.Any(sc => sc.SubjectId == subject.Id))
				return BadRequest("Student jest już zapisany na ten przedmiot.");

			// Utworzenie nowej relacji StudentCourse
			var newStudentCourse = new StudentCourse
			{
				StudentId = student.Id,
				SubjectId = subject.Id,
				StartingDate = DateOnly.FromDateTime(DateTime.Now)
			};

			// Dodanie nowej relacji do bazy danych
			_context.StudentCourses.Add(newStudentCourse);
			await _context.SaveChangesAsync();

			var createdStudentCourse = await _context.StudentCourses
				.Include(sc => sc.Student)
				.Include(sc => sc.Subject)
				.FirstOrDefaultAsync(sc => sc.Id == newStudentCourse.Id);

			// Zwrócenie wyniku
			return Ok(new StudentCourseDto(createdStudentCourse));
		}


		[HttpGet("subject")]
		public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsAsync()
		{
			return await _context.Subjects.ToListAsync();
		}

		[HttpPost("subject")]
		public async Task<ActionResult<Subject>> CreateSubjectAsync([FromBody] Subject subject)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectName == subject.SubjectName);
			if (existingSubject != null)
			{
				return Conflict($"Przedmiot o nazwie '{subject.SubjectName}' już istnieje.");
			}

			_context.Subjects.Add(subject);
			await _context.SaveChangesAsync();

			return Ok(subject);
		}
		[HttpDelete("subject/{subjectName}")]
		public async Task<ActionResult> DeleteSubjectAsync(string subjectName)
		{
			var subject = await _context.Subjects
				.FirstOrDefaultAsync(s => s.SubjectName == subjectName);

			if (subject == null)
			{
				return NotFound("nie ma tego");
			}
			_context.Subjects.Remove(subject);

			await _context.SaveChangesAsync();
			return Ok();

		}
		[HttpGet("subject/{subjectId}/students")]
		public async Task<ActionResult<IEnumerable<Student>>> GetStudentsBySubject(Guid subjectId)
		{
			var students = await _context.StudentCourses
				.Where(sc => sc.SubjectId == subjectId)
				.Include(sc => sc.Student)
				.Select(sc => sc.Student)
				.ToListAsync();

			

			return Ok(students);
		}
		private string GenerateCsv(IEnumerable<Student> students)
		{
			var csv = new StringBuilder();
			csv.AppendLine("FirstName,LastName,Email,Semester,Wiek");

			foreach (var student in students)
			{
				csv.AppendLine($"{student.FirstName},{student.LastName},{student.Email},{student.Semester},{student.Wiek}");
			}

			return csv.ToString();
		}


	}
}