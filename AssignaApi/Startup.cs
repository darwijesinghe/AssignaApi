using AssignaApi.Data;
using AssignaApi.Helpers;
using AssignaApi.Interfaces;
using AssignaApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace AssignaApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private const string cors = "AllowOrigin";

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: cors,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

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

                option.UseSqlServer(options.ConnectionString, action =>
                {
                    action.CommandTimeout(options.CommandTimeout);
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
                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetSection("JWTConfig:Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = _configuration.GetSection("JWTConfig:Audience").Value,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
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
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // unhandle error handeling
                app.UseExceptionHandler("/error");
            }

            // use swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(swg =>
            {
                swg.SwaggerEndpoint("/swagger/v1/swagger.json", "AssignaApi v1");
                swg.RoutePrefix = "swagger";
            });

            app.UseRouting();

            app.UseCors(cors);

            app.UseHttpsRedirection();

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