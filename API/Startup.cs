using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
            //Section4- Adding the below service TokenService at the top of the method: We need to tell 
            //our dependency injection container what is this service lifetime 
            //and howlong should the service be living after we started.
            //1. services.AddSingleton() --> it's created or instantiated is created and then
            //it doesn't stop until our application stops. 
            //So it continues to use resources not really appropriate for something like 
            //a service where we just want to create a token. Once the token is created, 
            //we don't need that service around anymore. So singleton is not good for this. 
            //2. services.AddScoped() --> it is scoped to the lifetime of the http request in this case.
            //Because we are using this inside the API controller. Whenever the request comes inn 
            //and we have the service injected into that particular controller,
            //then a new instance of the service is created, and when the request is finished, the service is disposed.
            //This is the one that we are going to use almost all the time. And this one is more appropriate for HTTP requests.
            //And since this is going to be used in the context of the HTTP requests.
            //3. services.AddTransient() --> The service is going to be created and, destroyed as soon as the method is finished.
            //And this one is normally considered not quite right for an HTTP request.

            //In the below line of code, it is not mandatory to use interface. But when we are testing our application, it is very easy to mock the interface.
            //We dont need to implement anything in interface. All We need is the signature of the method and then 
            //we can mock its behaviour when it comes to testing the application.   
            //services.AddScoped<ITokenService, TokenService>();

            //This is our dependency injection container. If we want to make a class or service available 
            //to other areas in the application, we can add them inside this container and 
            //.Net core is going to take care of the creation of these classes 
            //and destruction of these classes when no longer used. 
            // services.AddDbContext<DataContext>(options =>
            // {
            //     options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            // });

            //Moving the above two AddScoped & AddDbContext blocks to ApplicationServiceExtensions API extension method.
            services.AddApplicationServices(_config);

            services.AddControllers();
            //Adding CORS for other origins to access our services.
            services.AddCors();
            //Adding service for authentication. Also specifying the authentication scheme.
            //Here we have mentioned the parameters to be validated during authentication.
            //JWT bearer authentication performs authentication by extracting and validating a JWT token from the Authorization request header.
            //The signing key should be validated. so it is marked as true.
            //the issuer of the token is our API server and audience of the token is our angular app.
            //To be simpler, just marking these two attributes as false to not validate as of now.
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // .AddJwtBearer(options => {
            //     options.TokenValidationParameters =  new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
            //         ValidateIssuer = false,
            //         ValidateAudience = false,
            //     };
            // });

            //Moving the above AddAuthentication block to IdentityServiceExtensions API extension method.
            services.AddIdentityServices(_config);
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
            //This authentication middleware needs to be placed after CORS and before authorization.
            app.UseAuthentication();
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
