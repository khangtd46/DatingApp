using DatingApp.Data;
using DatingApp.Implementations;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContextPool<AppDBContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters()
		{
		  ValidateIssuerSigningKey = true,
		  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
		  ValidateIssuer = false,
		  ValidateAudience = false
		};
	}); //cái trong ngoặc đơn này gọi là authentication scheme
var app = builder.Build();

// Configure the HTTP request pipeline.




app.UseCors(builder =>
{
	builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
