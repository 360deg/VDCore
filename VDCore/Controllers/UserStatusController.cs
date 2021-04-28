using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VDCore.DBContext.Core;
using VDCore.DBContext.Core.Models;

namespace VDCore.Controllers
{
    [Route("[controller]")]
    public class UserStatusController: ControllerBase
    {
        private readonly CoreDbContext _context;
        public UserStatusController(CoreDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Returns raw userStatus list.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<UserStatus>>> GetList()
        {
            return await _context.UserStatus.ToListAsync();
        }
    }
}