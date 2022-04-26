using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaraSurvey.Core
{
    public static class DataBaseConfigs
    {
        public static void AddSqlServerDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DB>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            }, ServiceLifetime.Scoped);
        }
    }
}
