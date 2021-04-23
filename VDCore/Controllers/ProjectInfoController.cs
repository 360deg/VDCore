using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VDCore.Models.ProjectInfo;

namespace VDCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectInfoController : ControllerBase
    {
        
        private readonly ILogger<ProjectInfoController> _logger;

        public ProjectInfoController(ILogger<ProjectInfoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<Project> Get()
        {
            return new Project {ProjectName = "VDCore", Version = "v1.0.0", ReportDate = DateTime.Now};
        }
    }
}