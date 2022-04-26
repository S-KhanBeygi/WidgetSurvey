using AutoMapper;
using DaraSurvey.Core.BaseClasses;
using DaraSurvey.Entities;
using DaraSurvey.WidgetServices.Models;
using Newtonsoft.Json;
using System;

namespace DaraSurvey.Mappers
{
    public class WidgetProfile : Profile
    {
        public WidgetProfile()
        {
            CreateMap<EditModelBase, Widget>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src, JsonSeralizerSetting.SerializationSettings)));
        }
    }
}
