using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Services;
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
        // Cors
        const string cors = "AllowOrigin";

        // Services
        private readonly IConfiguration _configuration;

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
                        policy.WithOrigins(_configuration.GetSection("ClientApp:BaseUrl").Value)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            // Add IHttpClientFactory support
            services.AddHttpClient();

            services.AddControllers();

            services.AddHttpContextAccessor();

            // Add swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AssignaApi", Version = "v1" });

                // Define the Bearer token authentication scheme
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In          = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field.",
                    Name        = "Authorization",
                    Type        = SecuritySchemeType.ApiKey
                });

                // Add a global security requirement for the Bearer token
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // DB options
            services.ConfigureOptions<DatabaseOptionsSetup>();

            // DB service
            services.AddDbContext<DataContext>((serviceprovider, option) =>
            {
                // gets database option values
                var options = serviceprovider.GetService<IOptions<DatabaseOptions>>()!.Value;

                option.UseSqlServer(options.ConnectionString, action =>
                {
                    action.MigrationsAssembly("Infrastructure");
                    action.CommandTimeout(options.CommandTimeout);
                });
            });

            // JWT config setup
            services.ConfigureOptions<JwtConfigSetup>();

            // JWT authentication setup
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // JWT secret
                var keyBytes = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfig:Secret").Value);
                var key      = new SymmetricSecurityKey(keyBytes);

                options.SaveToken                 = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = key,
                    ValidateIssuer           = true,
                    ValidIssuer              = _configuration.GetSection("JWTConfig:Issuer").Value,
                    ValidateAudience         = true,
                    ValidAudience            = _configuration.GetSection("JWTConfig:Audience").Value,
                    RequireExpirationTime    = true,
                    ValidateLifetime         = true,
                    ClockSkew                = TimeSpan.Zero
                };
            });

            // get mail service setting values
            var mailconfig = _configuration.GetSection("MailConfigurations").Get<MailConfigurations>();
            services.AddSingleton(mailconfig);

            // client app setting values
            var clientApp = _configuration.GetSection("ClientApp").Get<ClientApp>();
            services.AddSingleton(clientApp);

            // Register Jwt service
            services.AddSingleton<IJwtService       , JwtService>();

            // Register mail service
            services.AddSingleton<IMailService      , MailService>();

            // Register repositories
            services.AddScoped<ITaskRepository      , TaskRepository>();
            services.AddScoped<ITeamLeadRepository  , TeamLeadRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<IUserRepository      , UserRepository>();

            // Register services
            services.AddScoped<ITaskService         , TaskService>();
            services.AddScoped<ITeamLeadService     , TeamLeadService>();
            services.AddScoped<ITeamMemberService   , TeamMemberService>();
            services.AddScoped<IUserService         , UserService>();
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
                // unhandled error handling
                app.UseExceptionHandler("/error");
            }

            // Use swagger UI
            app.UseSwagger();
            app.UseSwaggerUI(swg =>
            {
                swg.SwaggerEndpoint("/swagger/v1/swagger.json", "AssignaApi v1");
                swg.RoutePrefix = "swagger";
            });

            app.UseRouting();

            app.UseCors(cors);

            app.UseHttpsRedirection();

            // Aunthentication
            app.UseAuthentication();

            // Authorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Apply pending migrations
            ApplyMigrations(app);
        }

        /// <summary>
        /// Applies any pending database migrations at application startup to ensure
        /// the database schema is up-to-date with the latest changes
        /// </summary>
        /// <param name="app">The application builder instance used to configure the request pipeline</param>
        private static void ApplyMigrations(IApplicationBuilder app)
        {
            // Apply pending migrations automatically
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<DataContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}