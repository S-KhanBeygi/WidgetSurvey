using AutoMapper;
using DaraSurvey.Core.Filter;
using DaraSurvey.Models;
using DaraSurvey.Services.SurveryServices;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        // --------------------

        [HttpGet]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<IEnumerable<QuestionRes>> GetAll([FromQuery] QuestionOrderedFilter model)
        {
            var entities = _questionService.GetAll(model, true);
            var result = _mapper.Map<IEnumerable<QuestionRes>>(entities);
            return Ok(result);
        }

        // --------------------

        [HttpGet("count")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<int> Count([FromQuery] QuestionFilter model)
        {
            var result = _questionService.Count(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<QuestionRes> Get([FromRoute] int id)
        {
            var entity = _questionService.Get(id, true);
            var result = _mapper.Map<QuestionRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPost]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<QuestionRes> Create([FromBody] QuestionCreation model)
        {
            var entity = _questionService.Create(model);
            var result = _mapper.Map<QuestionRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPut("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<QuestionRes> Update([FromRoute] int id, [FromBody] QuestionUpdation model)
        {
            var entity = _questionService.Update(id, model);
            var result = _mapper.Map<QuestionRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpDelete("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult Delete([FromRoute] int id)
        {
            _questionService.Delete(id);
            return NoContent();
        }
    }
}