using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DaraSurvey.WidgetServices.Models
{
    public class EditModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext ctx)
        {
            if (ctx == null)
                throw new ArgumentNullException(nameof(ctx));

            StreamReader reader = new StreamReader(ctx.HttpContext.Request.Body);
            var value = reader.ReadToEnd();

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            try
            {
                var result = WidgetsDataDeserializer.Deserialize(value);
                ctx.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                ctx.Result = ModelBindingResult.Failed();
                ctx.ModelState.AddModelError("body", ex.Message);
                return Task.CompletedTask;
            }
        }
    }
}