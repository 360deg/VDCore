using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VDCore.DBContext;
using VDCore.Models.ProjectInfo;

namespace VDCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectInfoController : ControllerBase
    {
        private readonly CoreDbContext _context;
        
        //private readonly ILogger<ProjectInfoController> _logger;

        public ProjectInfoController(CoreDbContext context)
        {
            _context = context;
            if (!_context.Test.Any())
            {
                _context.Test.Add(new Test { ProjectName = "ALMAS" });
                _context.Test.Add(new Test { ProjectName = "VDCore" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<Project> GetProjectInfo()
        {
            //_context.Test.Add(new Test());
            return new Project {ProjectName = "VDCore", Version = "v1.0.0", ReportDate = DateTime.Now};
        }
        
        
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Test>>> GetProjectInfoFromDatabase()
        {
            return await _context.Test.ToListAsync();
        }
    }
}