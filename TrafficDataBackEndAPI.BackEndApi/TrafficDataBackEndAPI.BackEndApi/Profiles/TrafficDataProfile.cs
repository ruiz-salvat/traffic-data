using System;
using AutoMapper;

namespace TrafficDataBackEndAPI.BackEndApi.Profiles
{
    public class TrafficDataProfile:Profile
    {
        public TrafficDataProfile()
        {
            CreateMap<Tuple<Models.TrafficData, Models.MeasurementPoint>, DTO.TrafficData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.Item1.DateTime))
                .ForMember(dest => dest.Flow, opt => opt.MapFrom(src => src.Item1.Flow))
                .ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.Item1.Speed))
                .ForMember(dest => dest.MeasurementPointId, opt => opt.MapFrom(src => src.Item2.Id));
            CreateMap<DTO.TrafficData, Models.TrafficData>() 
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.Flow, opt => opt.MapFrom(src => src.Flow))
                .ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.Speed));
        }
    }
}