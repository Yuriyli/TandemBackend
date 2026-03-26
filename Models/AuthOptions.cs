using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TandemBackend.Models
{
    public static class AuthOptions
    {
        public static string ISSUER = "MyAuthServer";
        public static string AUDIENCE = "MyAuthClient";
        private static string KEY = "SupeawrSeawdcretCode*(&UANBWD98y92g3uygrol3rg333";

        public static void SetSeedKey(string? key)
        {
            if (key != null)
            {
                KEY = key;
            }
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
