using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
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

        public IConfiguration Configuration { get; }
        private readonly IConfiguration _projectInfo;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<CoreDbContext>(options =>
                options.UseMySql( 
                    Configuration.GetConnectionString("VDCoreConnection"), 
                    mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 1, 48), ServerType.MariaDb)
                    )
                );
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = _projectInfo.GetSection("ProjectName").Value, 
                    Version = _projectInfo.GetSection("Version").Value
                }); 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json", 
                    _projectInfo.GetSection("ProjectName").Value + " " + _projectInfo.GetSection("Version").Value
                    )
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}