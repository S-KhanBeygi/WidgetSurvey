using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Services.SurveryServices
{
    public interface IQuestionService
    {
        int Count(QuestionFilter model);
        Question Get(int id, bool enableIncludes = false);
        Question Create(QuestionDtoBase model);
        void Delete(int id);
        IEnumerable<Question> GetAll(QuestionOrderedFilter model, bool enableIncludes = false);
        IEnumerable<SurveyQuestion> GetQuestionData(int surveyId);
        Question Update(int id, QuestionDtoBase model);
    }
}