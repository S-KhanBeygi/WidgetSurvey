using DaraSurvey.Core;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class UserResponseFilter : FilterBase, IFilterable
    {
        public int? Id { get; set; }

        public string UserId { get; set; }

        public int? QuestionId { get; set; }

        public bool? IsCountable { get; set; }

        public string Response { get; set; }
    }

    // --------------------

    public class UserResponseOrderedFilter : UserResponseFilter, IOrderedFilterable
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Sort { get; set; }

        public bool Asc { get; set; }

        public bool RndArgmnt { get; set; }
    }
}
