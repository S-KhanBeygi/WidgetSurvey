using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DaraSurvey.Core
{
    public static class AppSettingReader
    {
        public static AppSettings GetAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(options => configuration.Bind(options));
            var provider = services.BuildServiceProvider();
            var siteSettingsOptions = provider.GetService<IOptionsSnapshot<AppSettings>>();
            var appSettings = siteSettingsOptions.Value;
            return appSettings;
        }
    }
}
