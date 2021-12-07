using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VDCore.Authorization
{
    /*
     * Required NuGet package: 
     * Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="5.0.5"
     */
    public class AuthOptions
    {
        public const string ISSUER = "VDCoreAuth";
        public const string AUDIENCE = "AuthClient";
        const string KEY = "vlad_pecherytsia_dev";
        public const int LIFETIME = 720;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
