using AutoMapper;
using DaraSurvey.Core;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DaraSurvey.Services.SurveryServices
{
    public class SurveyService : ISurveyService
    {
        private readonly DB _db;
        private readonly IMapper _mapper;

        public SurveyService(DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ------------------------

        public IEnumerable<SurveyOverviewRes> GetOverview(SurveyOverviewOrderedFilter model)
        {
            var surveyOrderedFilter = _mapper.Map<SurveyOrderedFilter>(model);
            var entities = Filter(surveyOrderedFilter)
                .Include(o => o.UsersSurvey.Where(s => s.UserId == model.UserId))
                .GetOrderedQuery(surveyOrderedFilter);

            var result = _mapper.Map<IEnumerable<SurveyOverviewRes>>(entities);

            return result;
        }

        // ----------------------

        public int OverviewCount(SurveyOverviewFilter model)
        {
            var surveyFilter = _mapper.Map<SurveyFilter>(model);
            var count = Filter(surveyFilter)
                .Include(o => o.UsersSurvey.Where(s => s.UserId == model.UserId))
                .Count();

            return count;
        }

        // ----------------------

        public IEnumerable<Survey> GetAll(SurveyOrderedFilter model)
        {
            return Filter(model).GetOrderedQuery(model);
        }

        // --------------------

        public int Count(SurveyFilter model)
        {
            return Filter(model).Count();
        }

        // --------------------

        public Survey Get(int id)
        {
            var entity = _db.Set<Survey>().SingleOrDefault(o => o.Id == id);
            if (entity == null)
                throw new ServiceException(HttpStatusCode.NotFound);

            return entity;
        }

        // --------------------

        public Survey Create(SurveyDtoBase model)
        {
            var entity = _mapper.Map<Survey>((SurveyCreation)model);

            _db.Set<Survey>().Add(entity);

            _db.SaveChanges();

            return entity;
        }

        // --------------------

        public Survey Update(int id, SurveyDtoBase model)
        {
            ThrowExceptionIfSurveyHasAcceptedUser(id);

            var entity = Get(id);

            entity = _mapper.Map((SurveyUpdation)model, entity);

            _db.Set<Survey>().Update(entity);
            _db.SaveChanges();

            return entity;
        }

        // --------------------

        public void Delete(int id)
        {
            ThrowExceptionIfSurveyHasAcceptedUser(id);
            ThrowExceptionIfSurveyHasAnyQuestion(id);

            var entity = Get(id);

            entity.Deleted = DateTime.UtcNow;

            _db.Set<Survey>().Update(entity);

            _db.SaveChanges();
        }

        // ********************************************************************* //
        //                           Helper Methods                              //
        // ********************************************************************* //
        private IQueryable<Survey> Filter(SurveyFilter model)
        {
            var q = _db.Set<Survey>().AsQueryable();

            if (model.Id.HasValue)
                q = q.Where(o => o.Id == model.Id);

            if (!string.IsNullOrEmpty(model.Title))
                q = q.Where(o => o.Title.Contains(model.Title));

            if (!string.IsNullOrEmpty(model.Description))
                q = q.Where(o => o.Description.Contains(model.Description));

            if (model.MinPublished.HasValue)
                q = q.Where(o => o.Published >= model.MinPublished);
            if (model.MaxPublished.HasValue)
                q = q.Where(o => o.Published <= model.MinPublished);

            if (model.MinExpired.HasValue)
                q = q.Where(o => o.Expired >= model.MinExpired);
            if (model.MaxExpired.HasValue)
                q = q.Where(o => o.Expired <= model.MaxExpired);

            if (model.MinExamStart.HasValue)
                q = q.Where(o => o.ExamStart >= model.MinExamStart);
            if (model.MaxExamStart.HasValue)
                q = q.Where(o => o.ExamStart <= model.MaxExamStart);

            if (model.MaxDuration.HasValue)
                q = q.Where(o => o.Duration >= model.MaxDuration);
            if (model.MinDuration.HasValue)
                q = q.Where(o => o.Duration <= model.MinDuration);

            if (!string.IsNullOrEmpty(model.SurveyDesignerId))
                q = q.Where(o => o.SurveyDesignerId == model.SurveyDesignerId);

            if (model.HasLogo.HasValue)
                q = q.Where(o => o.Logo.Length >= 1);

            if (model.MinCreated.HasValue)
                q = q.Where(o => o.Created >= model.MinCreated);
            if (model.MaxCreated.HasValue)
                q = q.Where(o => o.Created <= model.MaxCreated);

            if (model.MinUpdated.HasValue)
                q = q.Where(o => o.Updated >= model.MinUpdated);
            if (model.MaxUpdated.HasValue)
                q = q.Where(o => o.Updated <= model.MaxUpdated);

            if (model.WelcomePageWidgetId.HasValue)
                q = q.Where(o => o.WelcomePageWidgetId <= model.WelcomePageWidgetId);

            if (model.ThankYouPageWidgetId.HasValue)
                q = q.Where(o => o.ThankYouPageWidgetId <= model.ThankYouPageWidgetId);

            return q;
        }

        // ------------------------

        private void ThrowExceptionIfSurveyHasAcceptedUser(int surveyId)
        {
            var hasActiveUser = _db.Set<UsersSurvey>().Any(o => o.SurveyId == surveyId && o.Status != UserSurveyStatus.Requested);
            if (hasActiveUser)
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.SurveyHasActiveUser);
        }

        // ------------------------

        private void ThrowExceptionIfSurveyHasAnyQuestion(int surveyId)
        {
            var hasQuestion = _db.Set<Question>().Any(o => o.SurveyId == surveyId);
            if (hasQuestion)
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.DeleteSurveyQuestionsFirst);
        }
    }
}
