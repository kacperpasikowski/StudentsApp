using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		public DbSet<Student> Students { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<StudentCourse> StudentCourses { get; set; }
		public DbSet<Grade> Grades { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<StudentCourse>()
				.HasOne(sc => sc.Student)
				.WithMany(s => s.StudentCourses) 
				.HasForeignKey(sc => sc.StudentId)
				.IsRequired();

			builder.Entity<StudentCourse>()
				.HasOne(sc => sc.Subject)
				.WithMany(s => s.StudentCourses) 
				.HasForeignKey(sc => sc.SubjectId)
				.IsRequired();

			builder.Entity<Grade>()
				.HasOne(g => g.StudentCourse)
				.WithMany(sc => sc.Grades)
				.HasForeignKey(g => g.StudentCourseId)
				.IsRequired();
		}
	}
}