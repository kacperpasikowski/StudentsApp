using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
	public class AuthDataContext : IdentityDbContext
	{
		
		public AuthDataContext(DbContextOptions<AuthDataContext> options) : base(options)
		{
			
		}
		
		public DbSet<ApplicationUser> ApplicationUsers {get; set;}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			
			var studentRoleId = Guid.NewGuid().ToString();
			var secretaryRoleId = Guid.NewGuid().ToString();
			var adminRoleId = Guid.NewGuid().ToString();
			var viewerRoleId = Guid.NewGuid().ToString();
			
			
			var roles = new List<IdentityRole>
			{
				new IdentityRole()
				{
					Id = studentRoleId,
					Name = "Student",
					NormalizedName = "Student".ToUpper(),
					ConcurrencyStamp = studentRoleId
				},
				new IdentityRole()
				{
					Id = adminRoleId,
					Name = "Admin",
					NormalizedName = "Admin".ToUpper(),
					ConcurrencyStamp = adminRoleId
				},
				new IdentityRole()
				{
					Id = secretaryRoleId,
					Name = "Secretary",
					NormalizedName = "Secretary".ToUpper(),
					ConcurrencyStamp = secretaryRoleId
				},
				new IdentityRole()
				{
					Id = viewerRoleId,
					Name = "Viewer",
					NormalizedName = "Viewer".ToUpper(),
					ConcurrencyStamp = viewerRoleId
				}
			};
			
			builder.Entity<IdentityRole>().HasData(roles);
			
			var adminUserId = Guid.NewGuid().ToString();
			var admin = new ApplicationUser()
			{
				Id = adminUserId,
				UserName = "admin",
				NormalizedUserName = "admin".ToUpper(),
				Email = "admin@admin.com",
				NormalizedEmail = "admin@admin.com".ToUpper()
			};
			
			admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin@123");
			
			builder.Entity<ApplicationUser>().HasData(admin);
			
			var adminRoles = new List<IdentityUserRole<string>>()
			{
				new()
				{
					UserId = adminUserId,
					RoleId = secretaryRoleId,
				},
				new()
				{
					UserId = adminUserId,
					RoleId = adminRoleId,
				},
			};
			
			
			builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
			
			

		}
	}
}