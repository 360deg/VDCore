using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VDCore.Authorization;
using VDCore.DBContext.Core;

namespace VDCore.Controllers
{
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private CoreDbContext _context;
        public AuthController(CoreDbContext context)
        {
            _context = context;
        }
        
        /*
         * Method returns access_token for user if request successfully passed
         */
        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(string username, string password)
        {
            Console.WriteLine(_context.Database.ExecuteSqlRaw("Select 1"));
            var identity = new AuthIdentity().GetIdentity(username, password);
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
        
        
        [Authorize]
        [HttpGet]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
        }
         
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }
    }
}