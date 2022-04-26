using DaraSurvey.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Services.SurveryServices.Entities
{
    public class UserResponse : EntityBase<int>
    {
        public string UserId { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public bool IsCountable { get; set; }

        public string Response { get; set; }
    }

    // --------------------

    public class QuestionResponseConfiguration : IEntityTypeConfiguration<UserResponse>
    {
        public void Configure(EntityTypeBuilder<UserResponse> builder)
        {
            builder.ToTable("UserResponses")
                .HasQueryFilter(o => !o.Deleted.HasValue);
        }
    }
}
