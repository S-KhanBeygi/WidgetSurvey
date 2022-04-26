using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace DaraSurvey.Core.Filter
{
    public class MockAuthAttribute : Attribute, IActionFilter
    {
        public string Roles { get; set; }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var validRoles = Roles.Split(',');
            var identityClaims = context.HttpContext.User.Identity as ClaimsIdentity;
            var claimsRoles = identityClaims?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            if (!claimsRoles.Any() || claimsRoles == null)
                context.Result = new UnauthorizedResult();

            var hasRequiredRole = claimsRoles.Any(o => validRoles.Contains(o));
            if (!hasRequiredRole && !string.IsNullOrEmpty(Roles))
                context.Result = new ForbidResult();
        }
    }
}
