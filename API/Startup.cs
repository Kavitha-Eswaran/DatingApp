using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        readonly string MyAllowSpecificOrigins = "MyPolicy";
        public Startup(IConfiguration config)
        {
            _config = config;
            //Here our configuration is getting injected into the StartUp class.
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container. Ordering is not importent in this method.
        public void ConfigureServices(IServiceCollection services)
        {
            //This is our dependency injection container. If we want to make a class or service available 
            //to other areas in the application, we can add them inside this container and 
            //.Net core is going to take care of the creation of these classes and destruction of these classes when no longer used. 
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            });
            services.AddControllers();
            //Adding CORS for other origins to access our services.
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Ordering is very importent in this method.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If we are in development environment mode and our application encounters any problem, 
            //then we use the developer exception page  
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //if we hit the http url, then we will get redirected to https url.
            app.UseHttpsRedirection();
            //Routing is an action because we are able to go from browser the weatherforecast endpoint 
            //and get to our forecast controller. 
            app.UseRouting();

            //This is the right place to call the UseCors. Since the rule is it shoule be between UseRouting & UseEndPoints
            //and before authentication/authorization method.
            //Here we are defining our policy using expression with the parameter 'policy'.
            //Here we are allowing any header (we are going to be sending the header such as 'AuthenticationHeader to API from angular) 
            //and allowing any method get/put/post requests etc, but we are going to specific on the origin that they come from.   
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.UseAuthorization();
            //this middleware takes look inside the controller and see what endpoints are available. 
            //In our Weatherforecast controller, we have routing and HttpGet defined for an endpoint.  
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
