using DaraSurvey.Core;
using System;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class SurveyOverviewFilter
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

        public string UserId { get; set; }
    }

    // --------------------

    public class SurveyOverviewOrderedFilter : SurveyOverviewFilter, IOrderedFilterable
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Sort { get; set; }

        public bool Asc { get; set; }

        public bool RndArgmnt { get; set; }
    }

    // --------------------

    public class SurveyOverviewRes
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Expired { get; set; }

        public DateTime? ExamStart { get; set; }

        public SurveyOverviewStatus Status { get; set; }
    }


    // --------------------

    public enum SurveyOverviewStatus
    {
        Requestable,
        Requested,
        Approved,
        Disapproved
    }
}
