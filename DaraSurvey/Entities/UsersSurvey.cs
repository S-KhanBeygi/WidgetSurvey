using DaraSurvey.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Services.SurveryServices.Entities
{
    public class UsersSurvey : EntityBase<int>
    {
        public string UserId { get; set; }

        [ForeignKey("Survey")]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public DateTime? SurveyStartTime { get; set; }

        public TimeSpan? SurveyTimeTaken { get; set; }

        public UserSurveyStatus Status { get; set; }
    }

    // --------------------

    public class UsersSurveyConfiguration : IEntityTypeConfiguration<UsersSurvey>
    {
        public void Configure(EntityTypeBuilder<UsersSurvey> builder)
        {
            builder
                .ToTable("UsersSurvey")
                .HasQueryFilter(o => !o.Deleted.HasValue);
        }
    }

    // --------------------

    public enum UserSurveyStatus
    {
        Requested,
        Approved,
        Disapproved,
        Started,
        Finished
    }
}
