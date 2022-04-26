using DaraSurvey.Core;
using DaraSurvey.Entities;
using DaraSurvey.WidgetServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Services.SurveryServices.Entities
{
    public class Survey : EntityBase<int>
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Expired { get; set; }

        public DateTime? ExamStart { get; set; }

        public TimeSpan? Duration { get; set; }

        public TimeSpan? AllowedDelayTime { get; set; }

        public string SurveyDesignerId { get; set; }

        public string Logo { get; set; }

        [ForeignKey("WelcomePageWidget")]
        public int WelcomePageWidgetId { get; set; }
        public Widget WelcomePageWidget { get; set; }

        [ForeignKey("ThankYouPageWidget")]
        public int ThankYouPageWidgetId { get; set; }
        public Widget ThankYouPageWidget { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public IEnumerable<UsersSurvey> UsersSurvey { get; set; }
    }

    // --------------------

    public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder
                .ToTable("Surveys")
                .HasQueryFilter(o => !o.Deleted.HasValue);

            builder
                .HasOne(o => o.ThankYouPageWidget)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.WelcomePageWidget)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
