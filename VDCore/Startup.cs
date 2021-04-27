using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using VDCore.Authorization;
using VDCore.DBContext.Core;

namespace VDCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _projectInfo = Configuration.GetSection("ProjectInfo");
        }

        private IConfiguration Configuration { get; }
        private readonly IConfiguration _projectInfo;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<CoreDbContext>(options =>
                
            // DbContext options for MariaDB 10.1.48
            // For usage install NuGet packages:
            //
            // Include="Microsoft.Bcl.AsyncInterfaces" Version="6.0.0-preview.3.21201.4"
            // Include="Pomelo.EntityFrameworkCore.MySql" Version="3.2.5"
            //
            // ===========================================================================
            //
                options.UseMySql( 
                    Configuration.GetConnectionString("VDCoreConnection"), 
                    mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 1, 48), ServerType.MariaDb)
                )
            );
            
            // DbContext options for Latest Postgres database
            // For usage install NuGet packages:
            //
            // Include="EntityFramework" Version="6.4.4"
            // Include="Microsoft.EntityFrameworkCore" Version="6.0.0-preview.3.21201.2"
            // Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="6.0.0-preview3"
            //
            // ===========================================================================
            //
            // options.UseNpgsql(Configuration.GetConnectionString("PostgresLocal")));
            
                
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });
            
            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = _projectInfo.GetSection("ProjectName").Value, 
                        Version = _projectInfo.GetSection("Version").Value
                    }
                ); 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO remove comments after publish
            // if (env.IsDevelopment())
            // {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(
                        "/swagger/v1/swagger.json",
                        _projectInfo.GetSection("ProjectName").Value + " " + _projectInfo.GetSection("Version").Value
                    );
                });
            // TODO remove comments after publish
            // }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}