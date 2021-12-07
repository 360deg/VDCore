using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using VDCore.Authorization;
using VDCore.DBContext.Core;
using VDCore.Models;

namespace VDCore.Controllers
{
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CoreDbContext _context;
        public AuthController(CoreDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Returns access_token for user if request successfully passed.
        /// </summary>
        /// <param name="request">User login and password</param>
        /// <remarks>
        /// Sample value of response.
        /// 
        ///     {
        ///        "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodasd2",
        ///        "username": "admin"
        ///     }
        ///     
        /// </remarks>
        /// <response code="202">Accepted</response>  
        /// <response code="400">Bad request</response>  
        [HttpPost]
        [Route("[action]")]
        public IActionResult Login([FromBody] Login request)
        {
            var identity = new AuthIdentity(_context).GetIdentity(request.UserName, request.Password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            
            return Accepted(response);
        }
    }
}
