using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VDCore.DBContext.Core;
using VDCore.DBContext.Core.Models;

namespace VDCore.Authorization
{
    public class AuthIdentity
    {
        private readonly CoreDbContext _context;
        public AuthIdentity(CoreDbContext context)
        {
            _context = context;
        }
        
        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User usr = _context.Users.First(u => u.Login == username);
            if (!HashPasswordGenerator.VerifyHash(usr.Password, password))
            {
                return null;
            }

            string[] roleArray = _context.UserRoles.Where(ur => ur.UserId == usr.UserId).Select(ur => ur.Role.Name).ToArray();
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, usr.Login),
                new Claim(ClaimTypes.UserData, usr.CoreId.ToString())
            };

            foreach (var role in roleArray)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}