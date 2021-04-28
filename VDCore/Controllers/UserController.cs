using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VDCore.Authorization;
using VDCore.DBContext.Core;
using VDCore.DBContext.Core.Models;
using VDCore.Models.User;

namespace VDCore.Controllers
{
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly CoreDbContext _context;
        public UserController(CoreDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Returns raw user list.
        /// </summary>
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<User>>> GetList()
        {
            return await _context.Users.ToListAsync();
        }
        
        /// <summary>
        /// Returns raw user data by CoreID.
        /// </summary>
        /// <param name="coreId">unique guid for VDCore application</param>
        /// <response code="404">Not Found</response>  
        [Authorize]
        [HttpGet("{coreId:guid}")]
        public async Task<ActionResult<User>> GetUserById(Guid coreId)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.CoreId == coreId);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }
 
        /// <summary>
        /// NO AUTH REQUIRED. Adds new user to core system and returns created data.
        /// </summary>
        /// <remarks>
        /// New User will have default "User" role.
        /// </remarks>
        /// <response code="201">Created</response>  
        /// <response code="400">Bad request</response>  
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<User>> Add(UserRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { errorText = "No request data."});
            }

            if (_context.Users.Any(u => u.Login == request.Login))
            {
                return BadRequest(new { errorText = "User with that login already exists."});
            }
            
            if (!_context.UserStatus.Any(us => us.UserStatusId == request.UserStatusId))
            {
                return BadRequest(new { errorText = "Wrong UserStatusId."});
            }

            int userRole = _context.Roles.First(r => string.Equals(r.Name, "User", StringComparison.CurrentCultureIgnoreCase)).RoleId;
            
            User newUser = new User()
            {
                Login = request.Login,
                Password = HashPasswordGenerator.GenerateHash(request.Password),
                CoreId = Guid.NewGuid(),
                UserStatusId = request.UserStatusId
            };
            _context.Users.Add(newUser);

            _context.SaveChanges();
            
            UserRole newUserRole = new UserRole()
            {
                UserId = newUser.UserId,
                RoleId = userRole
            };
            _context.UserRoles.Add(newUserRole);
            
            
            await _context.SaveChangesAsync();

            Response.StatusCode = 201;
            return newUser;
        }
        
        /// <summary>
        /// Updates user data.
        /// </summary>
        /// <response code="400">Bad request</response>  
        /// <response code="404">Not found</response>  
        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult<User>> Update(UserUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { errorText = "No request data."});
            }
            
            if (!_context.Users.Any(x => x.CoreId == request.CoreId))
            {
                return NotFound(new { errorText = "User with coreId " + request.CoreId + " is not found."});
            }

            User userForUpdate = _context.Users.First(u => u.CoreId == request.CoreId);

            if (_context.Users.Any(u => u.Login == request.Login))
            {
                return BadRequest(new { errorText = "User with that login already exists."});
            }
            
            if (!_context.UserStatus.Any(us => us.UserStatusId == request.UserStatusId))
            {
                return BadRequest(new { errorText = "Wrong UserStatusId."});
            }

            // Updating user data.
            userForUpdate.Login = request.Login;
            userForUpdate.Password = HashPasswordGenerator.GenerateHash(request.Password);
            userForUpdate.UserStatusId = request.UserStatusId;
            _context.Update(userForUpdate);
            
            await _context.SaveChangesAsync();
            return Ok(userForUpdate);
        }
        
        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="coreId">unique guid for VDCore application</param>
        /// <response code="400">Bad request</response>  
        /// <response code="404">Not found</response>  
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{coreId:guid}")]
        public async Task<ActionResult<User>> Delete(Guid coreId)
        {
            if (!_context.Users.Any(x => x.CoreId == coreId))
            {
                return NotFound(new { errorText = "User with coreId " + coreId + " is not found."});
            }
            // TODO add Identity.CoreId
            // TODO add checker for not removing yourself
            
            User user = _context.Users.First(x => x.CoreId == coreId);
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}