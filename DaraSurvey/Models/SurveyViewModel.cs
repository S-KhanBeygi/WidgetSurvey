using DaraSurvey.WidgetServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public abstract class SurveyDtoBase : IValidatableObject
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

        public int WelcomePageWidgetId { get; set; }

        public int ThankYouPageWidgetId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var now = DateTime.UtcNow;

            if (Published.HasValue && Published.Value <= now)
                yield return new ValidationResult("Publish Date Cant Be Less Than Now DateTime");

            if (Expired.HasValue && Expired.Value <= now)
                yield return new ValidationResult("Expire Date Cant Be Less Than Now DateTime");

            if (ExamStart.HasValue && ExamStart.Value <= now)
                yield return new ValidationResult("ExamStart Date Cant Be Less Than Now DateTime");
        }
    }

    // --------------------

    public class SurveyCreation : SurveyDtoBase { }

    // ----------------------

    public class SurveyUpdation : SurveyDtoBase { }


    // ----------------------

    public class SurveyRes
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Expired { get; set; }

        public DateTime? ExamStart { get; set; }

        public TimeSpan? Duration { get; set; }

        public TimeSpan? AllowedDelayTime { get; set; }

        public string SurveyDesignerId { get; set; }

        public string Logo { get; set; }

        public ViewModelBase WelcomePageWidget { get; set; }

        public ViewModelBase ThankYouPageWidget { get; set; }
    }
}
