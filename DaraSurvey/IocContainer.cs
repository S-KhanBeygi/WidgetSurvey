using DaraSurvey.Services.SurveryServices;
using DaraSurvey.WidgetServices;
using Microsoft.Extensions.DependencyInjection;

namespace DaraSurvey.Core
{
    public static class IocContainer
    {
        public static void AddContainers(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IUserSurveyService, UserSurveyService>();
            services.AddScoped<IUserResponseService, UserResponseService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IWidgetService, WidgetService>();
        }
    }
}
