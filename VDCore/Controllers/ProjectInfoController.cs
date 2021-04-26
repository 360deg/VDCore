using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VDCore.Models.ProjectInfo;

namespace VDCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectInfoController : ControllerBase
    {
        public ProjectInfoController(IConfiguration iConfig)
        {
            Configuration = iConfig;
        }

        private IConfiguration Configuration { get; }
        
        [HttpGet]
        [Route("[action]")]
        public ActionResult<Project> GetProjectInfo()
        {
            var projectInfo = Configuration.GetSection("ProjectInfo");
            return new Project
            {
                ProjectName = projectInfo.GetSection("ProjectName").Value, 
                Version = projectInfo.GetSection("Version").Value, 
                ReportDate = DateTime.Now
            };
        }
    }
}