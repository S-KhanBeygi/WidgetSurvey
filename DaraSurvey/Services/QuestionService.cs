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
    public class QuestionService : IQuestionService
    {
        private readonly DB _db;
        private readonly IMapper _mapper;

        public QuestionService(DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ------------------------

        public IEnumerable<Question> GetAll(QuestionOrderedFilter model, bool enableIncludes = false)
        {
            var filter = enableIncludes
                ? Filter(model)
                    .Include(s => s.Survey)
                    .Include(s => s.Widget)
                : Filter(model);

            return filter.GetOrderedQuery(model);
        }

        // --------------------

        public int Count(QuestionFilter model)
        {
            return Filter(model)
                .Count();
        }

        // --------------------

        public Question Get(int id, bool enableIncludes = false)
        {
            var q = enableIncludes
                ? _db.Set<Question>().Include(s => s.Survey).AsQueryable()
                : _db.Set<Question>();

            var entity = q.SingleOrDefault(o => o.Id == id);
            if (entity == null)
                throw new ServiceException(HttpStatusCode.NotFound);

            return entity;
        }

        // --------------------

        public Question Create(QuestionDtoBase model)
        {
            var entity = _mapper.Map<Question>((QuestionCreation)model);

            _db.Set<Question>().Add(entity);

            _db.SaveChanges();

            return entity;
        }

        // --------------------

        public Question Update(int id, QuestionDtoBase model)
        {
            var entity = Get(id);

            ThrowExceptionIfSurveyHasAcceptedUser(entity.SurveyId);

            entity = _mapper.Map((QuestionUpdation)model, entity);

            _db.Set<Question>().Update(entity);
            _db.SaveChanges();

            return entity;
        }

        // --------------------

        public void Delete(int id)
        {
            var entity = Get(id);

            ThrowExceptionIfSurveyHasAcceptedUser(entity.SurveyId);

            entity.Deleted = DateTime.UtcNow;

            _db.Set<Question>().Update(entity);
            _db.SaveChanges();
        }

        // --------------------

        public IEnumerable<SurveyQuestion> GetQuestionData(int surveyId)
        {
            var entities = GetSurveyQuestions().ToList();

            return entities.Select(o => new SurveyQuestion { 
                IsCountable = o.IsCountable,
                IsRequired = o.IsRequired,
                QuestionId = o.Id,
                WidgetData = o.Widget.Data,
                Text = o.Text
            });

            #region localFunctions
            IEnumerable<Question> GetSurveyQuestions()
            {
                var questionOrderedFilter = new QuestionOrderedFilter
                {
                    SurveyId = surveyId,
                    RndArgmnt = true
                };

                var entities = GetAll(questionOrderedFilter, true);
                if (!entities.Any())
                    throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.SurveyHasNotAnyQuestion);

                return entities;
            }
            #endregion
        }
        // ********************************************************************* //
        //                           Helper Methods                              //
        // ********************************************************************* //

        private IQueryable<Question> Filter(QuestionFilter model)
        {
            var q = _db.Set<Question>().AsQueryable();

            if (model.Id.HasValue)
                q = q.Where(o => o.Id == model.Id);

            if (!string.IsNullOrEmpty(model.Text))
                q = q.Where(o => o.Text.Contains(model.Text));

            if (model.SurveyId.HasValue)
                q = q.Where(o => o.SurveyId == model.SurveyId);

            if (model.IsRequired.HasValue)
                q = q.Where(o => o.IsRequired == model.IsRequired);

            if (model.WidgetId.HasValue)
                q = q.Where(o => o.WidgetId == model.WidgetId);

            if (model.IsCountable.HasValue)
                q = q.Where(o => o.IsCountable == model.IsCountable);

            if (model.MinCreated.HasValue)
                q = q.Where(o => o.Created >= model.MinCreated);
            if (model.MaxCreated.HasValue)
                q = q.Where(o => o.Created <= model.MaxCreated);

            if (model.MinUpdated.HasValue)
                q = q.Where(o => o.Updated >= model.MinUpdated);
            if (model.MaxUpdated.HasValue)
                q = q.Where(o => o.Updated <= model.MaxUpdated);

            return q;
        }

        // ------------------------

        private void ThrowExceptionIfSurveyHasAcceptedUser(int surveyId)
        {
            var hasActiveUser = _db.Set<UsersSurvey>().Any(o => o.SurveyId == surveyId && o.Status != UserSurveyStatus.Requested);
            if (hasActiveUser)
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.SurveyHasActiveUser);
        }
    }
}
