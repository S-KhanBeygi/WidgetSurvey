using Microsoft.Extensions.DependencyInjection;

namespace DaraSurvey.Core.PackagesConfig
{
    public static class AddMvcConfiguration
    {
        public static void AddMvcConfigs(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelStateValidator());
            });
        }
    }
}
