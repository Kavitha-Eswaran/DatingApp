using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //In order to use this or extend the service collection that we're going to be returning, 
        //we need to use this keyword.So always specify 'this' before the type that you are extending.
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddDbContext<DataContext>(options =>
                  {
                      options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                  });
            return services;
        }
    }
}