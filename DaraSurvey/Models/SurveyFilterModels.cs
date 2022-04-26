using DaraSurvey.Core;
using System;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class SurveyFilter : FilterBase, IFilterable
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? MinPublished { get; set; }
        public DateTime? MaxPublished { get; set; }

        public DateTime? MinExpired { get; set; }
        public DateTime? MaxExpired { get; set; }

        public DateTime? MinExamStart { get; set; }
        public DateTime? MaxExamStart { get; set; }

        public TimeSpan? MinDuration { get; set; }
        public TimeSpan? MaxDuration { get; set; }

        public TimeSpan? MinAllowedDelayTime { get; set; }
        public TimeSpan? MaxAllowedDelayTime { get; set; }

        public int? WelcomePageWidgetId { get; set; }

        public int? ThankYouPageWidgetId { get; set; }

        public string SurveyDesignerId { get; set; }

        public bool? HasLogo { get; set; }
    }

    // --------------------

    public class SurveyOrderedFilter : SurveyFilter, IOrderedFilterable
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Sort { get; set; }

        public bool Asc { get; set; }

        public bool RndArgmnt { get; set; }
    }
}
