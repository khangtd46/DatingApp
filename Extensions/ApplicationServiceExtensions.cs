using DatingApp.Data;
using DatingApp.Implementations;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace DatingApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContextPool<AppDBContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnectionString"));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
