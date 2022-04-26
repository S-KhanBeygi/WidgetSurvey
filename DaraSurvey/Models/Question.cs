using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class QuestionDtoBase
    {
        public string Text { get; set; }

        public int WidgetId { get; set; }

        public int SurveyId { get; set; }

        public bool IsRequired { get; set; }

        public bool IsCountable { get; set; }
    }

    // --------------------

    public class QuestionCreation : QuestionDtoBase { }

    // --------------------

    public class QuestionUpdation : QuestionDtoBase { }


    // --------------------

    public class QuestionRes
    {
        public int Id { get; set; }

        public ViewModelBase Widget { get; set; }

        public bool IsRequired { get; set; }

        public string SurveyTitle { get; set; }

        public bool IsContable { get; set; }
    }
}
