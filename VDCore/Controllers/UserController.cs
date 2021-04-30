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
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetList()
        {
            return await _context.Users.Select(u => new UserResponse()
                {
                    Login = u.Login, 
                    Password = u.Password, 
                    UserStatusId = u.UserStatusId, 
                    CoreId = u.CoreId
                }
            ).ToListAsync();
        }
        
        /// <summary>
        /// Returns raw user data by CoreID.
        /// </summary>
        /// <param name="coreId">unique guid for VDCore application</param>
        /// <response code="404">Not Found</response>  
        [Authorize]
        [HttpGet("{coreId:guid}")]
        public async Task<ActionResult<UserResponse>> GetUserById(Guid coreId)
        {
            UserResponse user = await _context.Users.Where(u => u.CoreId == coreId).Select(u => new UserResponse()
                {
                    Login = u.Login, 
                    Password = u.Password, 
                    UserStatusId = u.UserStatusId, 
                    CoreId = u.CoreId
                }
            ).FirstAsync();
            
            if (user == null)
                return NotFound(new {errorText = "User not found."});
            return new ObjectResult(user);
        }
 
        /// <summary>
        /// NO AUTH REQUIRED. Adds new user to core system and returns created data.
        /// </summary>
        /// <param name="request">User sequence.</param>
        /// <remarks>
        /// New User will have default "User" role.
        /// </remarks>
        /// <response code="201">Created</response>  
        /// <response code="400">Bad request</response>  
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<UserResponse>> Add([FromBody] UserRequest request)
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
            return new UserResponse()
            {
                Login = newUser.Login, 
                Password = newUser.Password, 
                UserStatusId = newUser.UserStatusId, 
                CoreId = newUser.CoreId
            };
        }
        
        /// <summary>
        /// Updates user data.
        /// </summary>
        /// <param name="request">User sequence with coreId.</param>
        /// <response code="400">Bad request</response>  
        /// <response code="404">Not found</response>  
        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult<UserResponse>> Update([FromBody] UserUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { errorText = "No request data."});
            }
            
            if (!_context.Users.Any(x => x.CoreId == Guid.Parse(request.CoreId)))
            {
                return NotFound(new { errorText = "User with coreId " + request.CoreId + " is not found."});
            }

            User userForUpdate = _context.Users.First(u => u.CoreId == Guid.Parse(request.CoreId));

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
            return Ok( new UserResponse()
            {
                Login = userForUpdate.Login, 
                Password = userForUpdate.Password, 
                UserStatusId = userForUpdate.UserStatusId, 
                CoreId = userForUpdate.CoreId
            });
        }
        
        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <param name="coreId">unique guid for VDCore application</param>
        /// <response code="400">Bad request</response>  
        /// <response code="404">Not found</response>  
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{coreId:guid}")]
        public async Task<ActionResult<UserResponse>> Delete(Guid coreId)
        {
            if (!_context.Users.Any(x => x.CoreId == coreId))
            {
                return NotFound(new { errorText = "User with coreId " + coreId + " is not found."});
            }
            // TODO add Identity.CoreId
            // TODO add checker for not removing yourself
            
            User user = _context.Users.First(x => x.CoreId == coreId);
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(us => us.UserId == user.UserId));
            _context.SaveChanges();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok( new UserResponse()
            {
                Login = user.Login, 
                Password = user.Password, 
                UserStatusId = user.UserStatusId, 
                CoreId = user.CoreId
            });
        }
    }
}