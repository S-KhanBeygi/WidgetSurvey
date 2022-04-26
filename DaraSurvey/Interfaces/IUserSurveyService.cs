using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Services.SurveryServices
{
    public interface IUserSurveyService
    {
        void Approved(int userSurveyId, bool approved);
        int Count(UserSurveyFilter model);
        IEnumerable<UsersSurvey> GetAll(UserSurveyOrderedFilter model);
        IEnumerable<SurveyQuestion> GetSurveyQuestions(int userSurveyId, string userId);
        void Register(int surveyId, string userId);
        void SetSurveyResponses(int userSurveyId, string userId, IEnumerable<SurveyResponse> questionResponses);
    }
}