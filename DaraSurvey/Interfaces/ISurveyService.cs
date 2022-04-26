using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Services.SurveryServices
{
    public interface ISurveyService
    {
        IEnumerable<SurveyOverviewRes> GetOverview(SurveyOverviewOrderedFilter model);
        int OverviewCount(SurveyOverviewFilter model);
        int Count(SurveyFilter model);
        Survey Create(SurveyDtoBase model);
        void Delete(int id);
        Survey Get(int id);
        IEnumerable<Survey> GetAll(SurveyOrderedFilter model);
        Survey Update(int id, SurveyDtoBase model);
    }
}