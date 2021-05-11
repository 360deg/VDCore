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
        public async Task<IPagedList<UserStatus>> GetList(Pagination pagination)
        {
            return await _context.UserStatus.ToPagedListAsync(pagination.PageNumber, pagination.RowsOnPage);
        }
    }
}