using AutoMapper;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System;
using System.Linq;

namespace DaraSurvey.Services.SurveryServices.Mappers
{
    public class SurveyProfile : Profile
    {
        public SurveyProfile()
        {
            CreateMap<SurveyCreation, Survey>()
               .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ----------------------

            CreateMap<SurveyUpdation, Survey>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.Created, opt => opt.Ignore())
                .ForMember(o => o.Updated, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ----------------------

            CreateMap<SurveyOverviewFilter, SurveyFilter>();

            // ----------------------

            CreateMap<SurveyOverviewOrderedFilter, SurveyOrderedFilter>();

            // ----------------------

            CreateMap<Survey, SurveyOverviewRes>()
                .ForMember(o => o.Status, opt => opt.MapFrom(src => GetSurveyOverviewStatus(src)));

            // --------------------

            CreateMap<Survey, SurveyRes>();
        }

        // ----------------------

        private SurveyOverviewStatus GetSurveyOverviewStatus(Survey model)
        {
            if (model.UsersSurvey == null)
                return SurveyOverviewStatus.Requestable;

            var status = model.UsersSurvey.First().Status switch
            {
                UserSurveyStatus.Requested => SurveyOverviewStatus.Requestable,
                UserSurveyStatus.Disapproved => SurveyOverviewStatus.Disapproved,
                _ => SurveyOverviewStatus.Approved
            };

            return status;
        }
    }
}
