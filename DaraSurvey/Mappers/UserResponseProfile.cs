using AutoMapper;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System;

namespace DaraSurvey.Services.SurveryServices.Mappers
{
    public class UserResponseProfile : Profile
    {
        public UserResponseProfile()
        {
            CreateMap<UserResponseCreation, UserResponse>()
               .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
