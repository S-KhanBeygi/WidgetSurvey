using DaraSurvey.Core;
using DaraSurvey.Services.SurveryServices.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaraSurvey.Entities
{
    public class Widget : EntityBase<int>
    {
        public string Data { get; set; }
    }

    // ----------------------

    public class WidgetConfiguration : IEntityTypeConfiguration<Widget>
    {
        public void Configure(EntityTypeBuilder<Widget> builder)
        {
            builder
                .ToTable("Widgets")
                .HasQueryFilter(o => !o.Deleted.HasValue);
        }
    }
}
