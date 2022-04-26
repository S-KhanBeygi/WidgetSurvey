using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace DaraSurvey.Core
{
    public class ModelStateValidator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelStateErrors = context.ModelState.Values;
                var messages =
                    modelStateErrors.Select(o => o.Errors)
                    .SelectMany(x => x.Select(z => z.ErrorMessage))
                    .ToList();

                context.Result = new BadRequestObjectResult(new HandeledExceptionInfo
                {
                    ServiceExceptionCode = ServiceExceptionCode.ValidationError,
                    Messages = messages
                });
            }
        }
    }
}
