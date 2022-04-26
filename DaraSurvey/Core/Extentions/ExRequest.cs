using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DaraSurvey.Core.Request
{
    public static class ExRequest
    {
        public static IEnumerable<string> GetUserRoles(this HttpRequest request)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            var roles = identityClaims?.FindAll(ClaimTypes.Role).Select(o => o.Value);
            return roles;
        }
    }
}
