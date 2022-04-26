using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Services.SurveryServices
{
    public interface IUserResponseService
    {
        IEnumerable<UserResponse> Create(IEnumerable<UserResponseCreation> model);
    }
}