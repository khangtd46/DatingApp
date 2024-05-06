using DatingApp.Data;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using DatingApp.Middlewares;
using DatingApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddScoped<IUserRepository, UserRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.



app.UseExceptionMiddleware();
app.UseCors(builder =>
{
	builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scrope = app.Services.CreateScope();
var services = scrope.ServiceProvider;
try
{
	var context = services.GetRequiredService<AppDBContext>();
	await context.Database.MigrateAsync();
	await Seed.SeedUsers(context);
}
catch (Exception ex)
{
	var logger = services.GetService<ILogger<Program>>();
	logger.LogError(ex,"An Error occured durig migration");
}


app.Run();
