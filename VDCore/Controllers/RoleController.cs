using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VDCore.DBContext.Core;
using VDCore.DBContext.Core.Models;
using VDCoreLib;
using X.PagedList;

namespace VDCore.Controllers
{
    [Route("[controller]")]
    public class RoleController: ControllerBase
    {
        private readonly CoreDbContext _context;
        public RoleController(CoreDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Returns raw userRole list.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("[action]")]
        public async Task<IPagedList<Role>> GetList(Pagination pagination)
        {
            return await _context.Roles.ToPagedListAsync(pagination.PageNumber, pagination.RowsOnPage);
        }

        /// <summary>
        /// Returns current user's role list.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public List<string> GetCurrentRolesList()
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;
            var roles = claims.Where(c => c.Type == roleClaimType).Select(ro => ro.Value).ToList();
            return roles;
        }
        
        /// <summary>
        /// Checks if current user is administrator.
        /// </summary>
        /// <remarks>
        /// Returns <code>true</code> if success or error if not.
        /// </remarks>
        /// <response code="401">Unauthorized</response>  
        /// <response code="403">Forbidden: User is not administrator</response>  
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route("[action]")]
        public bool IsUserAdmin()
        {
            return true;
        }
    }
}