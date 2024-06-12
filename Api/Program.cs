using System.Text;
using Api.Data;
using Api.Entities;
using Api.Repositories.Interface;
using Api.Repositories.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt =>
{
	opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddDbContext<AuthDataContext>(opt =>
{
	opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddCors();


builder.Services.AddIdentityCore<ApplicationUser>()
	.AddRoles<IdentityRole>()
	.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("StudentApp")
	.AddEntityFrameworkStores<AuthDataContext>()
	.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt =>
{
	opt.Password.RequireDigit = true;
	opt.Password.RequireLowercase = true;
	opt.Password.RequireNonAlphanumeric = true;
	opt.Password.RequiredLength = 8;
	opt.Password.RequiredUniqueChars = 1;

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
			ValidateIssuer = false,
			ValidateAudience = false,
		};
	});



var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors(opt =>
{
	opt.AllowAnyHeader()
	   .AllowAnyOrigin()
	   .AllowAnyMethod()
	   .AllowCredentials()
	   .WithExposedHeaders("Pagination")
	   .WithOrigins("https://localhost:4200");
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
