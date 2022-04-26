using DaraSurvey.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DaraSurvey.Core.Filter
{
    public class MockUserAttribute : Attribute, IActionFilter
    {
        public Role Role { get; set; }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var nameIdentifire = Role == Role.root
                    ? "23ffd0b0-0a63-4948-8152-1c32743f79d8"
                    : "66f904c7-949c-48c6-9e98-86e24303f9dd";

            var name = Role == Role.root
                ? "root user"
                : "normal user";

            var email = Role == Role.root
                ? "root user"
                : "normal user";

            var role = Role == Role.root
                ? "root"
                : "users";

            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nameIdentifire),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            }));

        }
    }
}
