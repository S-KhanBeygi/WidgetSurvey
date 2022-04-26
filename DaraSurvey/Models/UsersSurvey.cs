namespace DaraSurvey.Services.SurveryServices.Models
{
    public abstract class UserSurveyDtoBase
    {
        public string UserId { get; set; }
        public int SurveyId { get; set; }
    }

    // --------------------

    public class UserSurveyCreation : UserSurveyDtoBase { }

    // --------------------

    public class UserSurveyUpdation : UserSurveyDtoBase { }
}
