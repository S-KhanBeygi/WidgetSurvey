using DaraSurvey.Core;
using System;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class UserSurveyFilter : FilterBase, IFilterable
    {
        public int? Id { get; set; }

        public string UserId { get; set; }

        public int? SurveyId { get; set; }

        public DateTime? MinSurveyStartTime { get; set; }
        public DateTime? MaxSurveyStartTime { get; set; }

        public TimeSpan? MinSurveyTimeTaken { get; set; }
        public TimeSpan? MaxSurveyTimeTaken { get; set; }
    }

    // --------------------

    public class UserSurveyOrderedFilter : UserSurveyFilter, IOrderedFilterable
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Sort { get; set; }

        public bool Asc { get; set; }

        public bool RndArgmnt { get; set; }
    }
}
