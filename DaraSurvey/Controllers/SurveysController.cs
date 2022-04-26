using AutoMapper;
using DaraSurvey.Core.Filter;
using DaraSurvey.Extentions;
using DaraSurvey.Models;
using DaraSurvey.Services.SurveryServices;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/surveys")]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IUserSurveyService _userSurveyService;
        private readonly IMapper _mapper;

        public SurveysController(ISurveyService surveyService, IUserSurveyService userSurveyService, IMapper mapper)
        {
            _surveyService = surveyService;
            _userSurveyService = userSurveyService;
            _mapper = mapper;
        }

        // --------------------

        [HttpGet("overview")]
        [MockUser(Role = Role.users)]
        [MockAuth(Roles = "users")]
        public ActionResult<IEnumerable<SurveyOverviewRes>> GetOverview([FromQuery] SurveyOverviewOrderedFilter model)
        {
            model.UserId = Request.GetUserId();
            var result = _surveyService.GetOverview(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("overview/count")]
        [MockUser(Role = Role.users)]
        [MockAuth(Roles = "users")]
        public ActionResult<int> GetOverviewCount([FromQuery] SurveyOverviewFilter model)
        {
            model.UserId = Request.GetUserId();
            var result = _surveyService.OverviewCount(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<Survey> GetAll([FromQuery] SurveyOrderedFilter model)
        {
            var result = _surveyService.GetAll(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("count")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<Survey> Count([FromQuery] SurveyFilter model)
        {
            var result = _surveyService.Count(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<SurveyRes> Get([FromRoute] int id)
        {
            var entity = _surveyService.Get(id);
            var result = _mapper.Map<SurveyRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPost("{surveyId}/register")]
        [MockUser(Role = Role.users)]
        [MockAuth(Roles = "users")]
        public ActionResult Register([FromRoute] int surveyId)
        {
            _userSurveyService.Register(surveyId, Request.GetUserId());
            return NoContent();
        }

        // --------------------

        [HttpPost]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<SurveyRes> Create([FromBody] SurveyCreation model)
        {
            model.SurveyDesignerId = Request.GetUserId();
            var entity = _surveyService.Create(model);
            var result = _mapper.Map<SurveyRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPut("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<SurveyRes> Update([FromRoute] int id, [FromBody] SurveyUpdation model)
        {
            var entity = _surveyService.Update(id, model);
            var result = _mapper.Map<SurveyRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpDelete("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult Delete([FromRoute] int id)
        {
            _surveyService.Delete(id);
            return NoContent();
        }

        // --------------------

        [HttpGet("{id}/questions")]
        [MockUser(Role = Role.users)]
        [MockAuth(Roles = "users")]
        public ActionResult<IEnumerable<SurveyQuestion>> GetSurveyQuestions([FromRoute] int id)
        {
            var result = _userSurveyService.GetSurveyQuestions(id, Request.GetUserId());
            return Ok(result);
        }

        // --------------------

        [HttpPost("{id}/responses")]
        [MockUser(Role = Role.users)]
        [MockAuth(Roles = "users")]
        public ActionResult SetSurveyResponses([FromRoute] int id, [FromBody] IEnumerable<SurveyResponse> model)
        {
            _userSurveyService.SetSurveyResponses(id, Request.GetUserId(), model);
            return NoContent();
        }
    }
}