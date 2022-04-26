using DaraSurvey.Entities;
using DaraSurvey.Services.SurveryServices.Entities;
using Microsoft.EntityFrameworkCore;

namespace DaraSurvey.Core
{
    public class DB : DbContext
    {
        public DB(DbContextOptions<DB> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // ------------------

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .ApplyConfiguration(new QuestionConfiguration())
                .ApplyConfiguration(new QuestionResponseConfiguration())
                .ApplyConfiguration(new SurveyConfiguration())
                .ApplyConfiguration(new WidgetConfiguration())
                .ApplyConfiguration(new UsersSurveyConfiguration());
        }
    }
}
