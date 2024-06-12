using System.Security.Claims;

namespace SimpleASPNetMVC
{
    public class Util
    {
        public static string GetLoggedInUserEmail(ClaimsPrincipal user) 
        {
            var email = string.Empty;
            var identity = (ClaimsIdentity)user.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            foreach (var claim in claims)
            {
                if (claim.Type.Equals(ClaimTypes.Email))
                {
                    email = claim.Value;
                    break;
                }
            }

            return email;
        }
    }
}
