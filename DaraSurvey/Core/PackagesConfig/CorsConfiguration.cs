using Microsoft.Extensions.DependencyInjection;

namespace DaraSurvey.Core.PackagesConfig
{
    public static class CorsConfiguration
    {
        public static void AddCorsConfigs(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCors", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .Build();
                });
            });
        }
    }
}
