using System.Collections.Generic;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class ValidResponse
    {
        public int QuestionId { get; set; }
        public IEnumerable<string> ValidResponses { get; set; }
    }
}
