using AutoMapper;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System;

namespace DaraSurvey.Services.SurveryServices.Mappers
{
    public class UserSurveyProfile : Profile
    {
        public UserSurveyProfile()
        {
            CreateMap<UserSurveyCreation, UsersSurvey>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserSurveyStatus.Requested))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ----------------------

            CreateMap<UserSurveyUpdation, UsersSurvey>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.Created, opt => opt.Ignore())
                .ForMember(o => o.Updated, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
