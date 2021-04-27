using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VDCore.Controllers;

namespace VDCore.Authorization
{
    public class AuthIdentity
    {
        // тестовые данные вместо использования базы данных
        private List<Person> people = new List<Person>
        {
            new Person { Login="admin@gmail.com", Password="12345", Role = "admin" },
            new Person { Login="qwerty@gmail.com", Password="55555", Role = "user" },
            new Person { Login="1", Password="1", Role = "admin" }
        };
        
        public ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
 
            // If user not found
            return null;
        }
    }
}