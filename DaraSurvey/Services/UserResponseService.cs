using AutoMapper;
using DaraSurvey.Core;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Services.SurveryServices
{
    public class UserResponseService : IUserResponseService
    {
        private readonly DB _db;
        private readonly IMapper _mapper;

        public UserResponseService(DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ------------------------

        public IEnumerable<UserResponse> Create(IEnumerable<UserResponseCreation> model)
        {
            var entities = _mapper.Map<IEnumerable<UserResponse>>(model);

            _db.Set<UserResponse>().AddRange(entities);

            _db.SaveChanges();

            return entities;
        }
    }
}
