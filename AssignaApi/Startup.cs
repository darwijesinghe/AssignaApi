using AssignaApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AssignaApi.Interfaces;
using AssignaApi.Services;
using AssignaApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AssignaApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            // add swagger
            services.AddSwaggerGen(swg =>
            {
                swg.SwaggerDoc("v1", new OpenApiInfo { Title = "AssignaApi", Version = "v1" });
            });

            // database options
            services.ConfigureOptions<DatabaseOptionsSetup>();

            // database service
            services.AddDbContext<DataContext>((serviceprovider, option) =>
            {
                // get database option values
                var options = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                option.UseSqlServer(options.connectionString, action =>
                {
                    action.CommandTimeout(options.commandTimeout);
                });
            });

            // JWT config setup
            services.ConfigureOptions<JwtConfigSetup>();

            // JWT authentication setup
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // JWT secret
                var keyBytes = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfig:Secret").Value);
                var key = new SymmetricSecurityKey(keyBytes);

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

            // get mail service setting values
            var mailconfig = _configuration.GetSection("MailConfigurations").Get<MailConfigurations>();
            services.AddSingleton(mailconfig);

            #region dependency services

            // database
            services.AddScoped<IDataService, DataService>();

            // mail
            services.AddScoped<IMailService, MailService>();

            // helper
            services.AddScoped<Helper>();

            // JWT helper
            services.AddScoped<JwtHelpers>();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // use swagger UI
                app.UseSwagger();
                app.UseSwaggerUI(swg =>
                {
                    swg.SwaggerEndpoint("/swagger/v1/swagger.json", "AssignaApi v1");
                    swg.RoutePrefix = string.Empty;
                });

                app.UseDeveloperExceptionPage();
            }
            else
            {
                // unhandle error handeling
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // aunthentication
            app.UseAuthentication();

            // authorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
