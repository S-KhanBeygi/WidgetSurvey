using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DaraSurvey.Extentions
{
    public static class ExIdentity
    {
        public static string GetUserId(this HttpRequest request)
        {
            var claim = GetClaim(request, ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        // ----------------------

        public static Claim GetClaim(this HttpRequest request, string claimType)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            return identityClaims?.FindFirst(claimType);
        }
    }
}
