using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class SurveyQuestion
    {
        public int QuestionId { get; set; }

        public string Text { get; set; }

        public bool IsRequired { get; set; }

        public bool IsCountable { get; set; }

        public string WidgetData { get; set; }

        public ViewModelBase Widget { get; set; }
    }

    // ------------------------

    public class SurveyResponse
    {
        public int QuestionId { get; set; }
        public string Response { get; set; }
    }
}
