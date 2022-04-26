namespace DaraSurvey.Services.SurveryServices.Models
{
    public class UserResponseCreation
    {
        public string UserId { get; set; }

        public int QuestionId { get; set; }

        public bool IsCountable { get; set; }

        public string Response { get; set; }
    }
}
