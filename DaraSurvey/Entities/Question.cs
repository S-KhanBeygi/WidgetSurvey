using DaraSurvey.Core;
using DaraSurvey.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Services.SurveryServices.Entities
{
    public class Question : EntityBase<int>
    {
        public string Text { get; set; }

        [ForeignKey("Widget")]
        public int WidgetId { get; set; }
        public Widget Widget { get; set; }

        [ForeignKey("Survey")]
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public bool IsRequired { get; set; }

        public bool IsCountable { get; set; }
    }

    // ----------------------

    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder
                .ToTable("Questions")
                .HasQueryFilter(o => !o.Deleted.HasValue);

            builder
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions);

            builder
               .HasOne(o => o.Widget)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
