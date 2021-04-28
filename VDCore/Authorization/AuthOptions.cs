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
        /*
         * Token issuer
         */
        public const string ISSUER = "VDCoreAuth";
        /*
         * Token issuer
         */
        public const string AUDIENCE = "AuthClient";
        /*
         * Token secret key
         */
        const string KEY = "vlad_pecherytsia_dev";
        /*
         * Token lifetime in minutes
         * 720 minutes == 12 hours
         */
        public const int LIFETIME = 720;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}