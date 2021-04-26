using System;
using Microsoft.AspNetCore.Mvc;
using VDCore.DBContext.Core;
using VDCore.Models.ProjectInfo;

namespace VDCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectInfoController : ControllerBase
    {
        public ProjectInfoController(CoreDbContext context)
        {
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Project> GetProjectInfo()
        {
            return new Project {ProjectName = "VDCore", Version = "v1.0.0", ReportDate = DateTime.Now};
        }
    }
}