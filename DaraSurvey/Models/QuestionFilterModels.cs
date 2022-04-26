using DaraSurvey.Core;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class QuestionFilter : FilterBase, IFilterable
    {
        public int? Id { get; set; }

        public string Text { get; set; }

        public int? SurveyId { get; set; }

        public int? WidgetId { get; set; }

        public bool? IsRequired { get; set; }

        public bool? IsCountable { get; set; }
    }

    // --------------------

    public class QuestionOrderedFilter : QuestionFilter, IOrderedFilterable
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Sort { get; set; }

        public bool Asc { get; set; }

        public bool RndArgmnt { get; set; }
    }
}
