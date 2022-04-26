using AutoMapper;
using DaraSurvey.Core;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using DaraSurvey.WidgetServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DaraSurvey.Services.SurveryServices
{
    public class UserSurveyService : IUserSurveyService
    {
        private readonly DB _db;
        private readonly IMapper _mapper;
        private readonly ISurveyService _surveyService;
        private readonly IQuestionService _questionService;
        private readonly IWidgetService _widgetService;
        private readonly IUserResponseService _userResponseService;

        public UserSurveyService(
            DB db,
            IMapper mapper,
            ISurveyService surveyService,
            IQuestionService questionService,
            IWidgetService widgetService,
            IUserResponseService userResponseService)
        {
            _db = db;
            _mapper = mapper;
            _surveyService = surveyService;
            _questionService = questionService;
            _widgetService = widgetService;
            _userResponseService = userResponseService;
        }

        // ----------------------

        public IEnumerable<UsersSurvey> GetAll(UserSurveyOrderedFilter model)
        {
            return Filter(model).GetOrderedQuery(model);
        }

        // --------------------

        public int Count(UserSurveyFilter model)
        {
            return Filter(model).Count();
        }

        // --------------------

        public void Register(int surveyId, string userId)
        {
            ThrowExceptionIfAlreadyHasThisSurvey(surveyId, userId);

            var survey = _surveyService.Get(surveyId);

            ThrowExceptinIfSurveyAlreadyExpired(survey);
            ThrowExceptionIfSurveyExamStarted(survey);

            RegisterUserSurvey();

            #region local functions
            void RegisterUserSurvey()
            {
                var userSurveyCreation = new UserSurveyCreation
                {
                    SurveyId = surveyId,
                    UserId = userId
                };

                Create(userSurveyCreation);
            }
            #endregion
        }

        // ----------------------

        public void Approved(int userSurveyId, bool approved)
        {
            var userSurvey = Get(userSurveyId);

            ThrowExceptionIfUserSurveyHasNorRequestedStatus(userSurvey);

            var survey = _surveyService.Get(userSurvey.SurveyId);

            ThrowExceptinIfSurveyAlreadyExpired(survey);
            ThrowExceptionIfSurveyExamStarted(survey);

            DoApproved();

            #region local functions
            void DoApproved()
            {
                var userSurveyStatus = approved
                    ? UserSurveyStatus.Approved
                    : UserSurveyStatus.Disapproved;

                userSurvey.Status = userSurveyStatus;

                _db.Set<UsersSurvey>().Update(userSurvey);

                _db.SaveChanges();
            }
            #endregion
        }

        // ----------------------

        public IEnumerable<SurveyQuestion> GetSurveyQuestions(int userSurveyId, string userId)
        {
            var userSurvey = Get(userSurveyId);

            ThrowExceptionIfUserSurveyHasNotApproveStatus();

            var survey = _surveyService.Get(userSurvey.SurveyId);

            var questionData = GetUserSurveyQuestions(userSurvey, userId, survey);

            SetExamStarted();

            return questionData;

            #region local functions
            void ThrowExceptionIfUserSurveyHasNotApproveStatus()
            {
                if (userSurvey.Status == UserSurveyStatus.Requested || userSurvey.Status == UserSurveyStatus.Disapproved)
                    throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.RequestedSurveyHasNotApproveStatus);
            }
            void SetExamStarted()
            {
                userSurvey.SurveyStartTime = DateTime.UtcNow;
                userSurvey.Status = UserSurveyStatus.Started;

                _db.Set<UsersSurvey>().Update(userSurvey);
                _db.SaveChanges();
            }
            #endregion
        }

        // ----------------------

        public void SetSurveyResponses(int userSurveyId, string userId, IEnumerable<SurveyResponse> userResponses)
        {
            var userSurvey = Get(userSurveyId);

            ThrowExceptionIfUserSurveyHasNotStartedStatus();

            var survey = _surveyService.Get(userSurvey.SurveyId);

            ThrowExceptionIfExamTimeIsFinished();

            var questions = GetUserSurveyQuestions(userSurvey, userId, survey);

            ThrowExceptionIfResponseQuestionsInvalid();

            ThrowExceptionIfSurveyResponseInvalid();

            SetUserResponses(questions, userResponses, userId);

            UpdateUserSurveyData();

            #region local functions
            void ThrowExceptionIfExamTimeIsFinished()
            {
                if (survey.ExamStart.HasValue && survey.Duration.HasValue)
                {
                    var finishTime = survey.ExamStart.Value.Add(survey.Duration.Value);
                    var now = DateTime.UtcNow;
                    if (finishTime >= now)
                        throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.ExamTimeIsFinished);
                }
            }
            void ThrowExceptionIfUserSurveyHasNotStartedStatus()
            {
                if (userSurvey.Status != UserSurveyStatus.Started)
                    throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.RequestedSurveyHasNotStartedStatus);
            }
            void ThrowExceptionIfResponseQuestionsInvalid()
            {
                var validQuestionIds = questions.Select(o => o.QuestionId);

                var responseQuestionIds = userResponses.Select(o => o.QuestionId);

                var exceptedQuestionIds = validQuestionIds.Except(responseQuestionIds).Count();

                if (exceptedQuestionIds >= 1 || validQuestionIds.Count() != responseQuestionIds.Count())
                    throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.InvalidResponseQuestionId);
            }
            void ThrowExceptionIfSurveyResponseInvalid()
            {
                foreach (var question in questions)
                {
                    var userResponse = userResponses.First(o => o.QuestionId == question.QuestionId).Response;

                    if (question.IsRequired || (!question.IsRequired && !string.IsNullOrEmpty(userResponse)))
                    {
                        if (!question.Widget.UserResponseIsValid(userResponse))
                            throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.InvalidSurveyResponse);
                    }
                }
            }
            void SetUserResponses(IEnumerable<SurveyQuestion> questions, IEnumerable<SurveyResponse> responses, string userId)
            {
                var userResponses = new List<UserResponseCreation>();

                questions.ToList().ForEach(o =>
                {
                    var userResponse = new UserResponseCreation
                    {
                        UserId = userId,
                        IsCountable = o.IsCountable,
                        QuestionId = o.QuestionId,
                        Response = responses.FirstOrDefault(r => r.QuestionId == o.QuestionId)?.Response
                    };
                    userResponses.Add(userResponse);

                });

                _userResponseService.Create(userResponses);
            }
            void UpdateUserSurveyData()
            {
                var now = DateTime.UtcNow;
                userSurvey.Updated = now;
                userSurvey.SurveyTimeTaken = now - userSurvey.SurveyStartTime.Value;
                userSurvey.Status = UserSurveyStatus.Finished;

                _db.Set<UsersSurvey>().Update(userSurvey);
                _db.SaveChanges();
            }
            #endregion
        }

        // ********************************************************************* //
        //                           Helper Methods                              //
        // ********************************************************************* //
        private UsersSurvey Get(int id)
        {
            var entity = _db.Set<UsersSurvey>().SingleOrDefault(o => o.Id == id);
            if (entity == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.QuestionsNotFound);

            return entity;
        }

        // --------------------

        private UsersSurvey Create(UserSurveyDtoBase model)
        {
            var entity = _mapper.Map<UsersSurvey>((UserSurveyCreation)model);

            _db.Set<UsersSurvey>().Add(entity);

            _db.SaveChanges();

            return entity;
        }

        // --------------------

        private IQueryable<UsersSurvey> Filter(UserSurveyFilter model)
        {
            var q = _db.Set<UsersSurvey>().AsQueryable();

            if (model.Id.HasValue)
                q = q.Where(o => o.Id == model.Id);

            if (!string.IsNullOrEmpty(model.UserId))
                q = q.Where(o => o.UserId == model.UserId);

            if (model.SurveyId.HasValue)
                q = q.Where(o => o.SurveyId == model.SurveyId);

            if (model.MinSurveyStartTime.HasValue)
                q = q.Where(o => o.SurveyStartTime >= model.MinSurveyStartTime);
            if (model.MaxSurveyStartTime.HasValue)
                q = q.Where(o => o.SurveyStartTime <= model.MaxSurveyStartTime);

            if (model.MinSurveyTimeTaken.HasValue)
                q = q.Where(o => o.SurveyTimeTaken >= model.MinSurveyTimeTaken);
            if (model.MaxSurveyTimeTaken.HasValue)
                q = q.Where(o => o.SurveyTimeTaken <= model.MaxSurveyTimeTaken);

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

        // ----------------------

        private void ThrowExceptinIfSurveyAlreadyExpired(Survey survey)
        {
            var now = DateTime.UtcNow;

            if (survey.Expired.HasValue && survey.Expired >= now)
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.SurveyAlreadyExpired);
        }

        // ----------------------

        private void ThrowExceptionIfSurveyExamStarted(Survey survey)
        {
            var now = DateTime.UtcNow;

            if (survey.ExamStart.HasValue)
            {
                var throwException = survey.AllowedDelayTime.HasValue
                    ? survey.ExamStart.Value.Add(survey.AllowedDelayTime.Value) >= now
                    : survey.ExamStart.Value >= now;

                if (throwException)
                    throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.SurveyExamAlreadyStarted);
            }
        }

        // ----------------------

        private IEnumerable<SurveyQuestion> GetUserSurveyQuestions(UsersSurvey userSurvey, string userId, Survey survey)
        {
            ThrowExceptionIfRequestedSurveyInvalid();
            ThrowExceptinIfSurveyAlreadyExpired(survey);
            ThrowExceptionIfSurveyExamStarted(survey);

            var questionData = _questionService.GetQuestionData(survey.Id)
                .Select(o => { 
                    o.Widget = _widgetService.GetWidget(o.WidgetData);
                    return o;
                });

            return questionData;

            #region local functions
            void ThrowExceptionIfRequestedSurveyInvalid()
            {
                if (userSurvey.UserId != userId)
                    throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.InvalidRequestedSurvey);
            }
            #endregion
        }

        // ----------------------

        private void ThrowExceptionIfAlreadyHasThisSurvey(int surveryId, string userId)
        {
            var filter = new UserSurveyOrderedFilter
            {
                UserId = userId,
                SurveyId = surveryId
            };

            var userSurvey = GetAll(filter).FirstOrDefault();

            if (userSurvey != null)
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.AlreadyHasThisSurvey);
        }

        // ----------------------

        private void ThrowExceptionIfUserSurveyHasNorRequestedStatus(UsersSurvey model)
        {
            if (model.Status != UserSurveyStatus.Requested)
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.UserSurveyHasNotRequestedStatus);
        }
    }
}
