using AutoMapper;

namespace TrafficDataBackEndAPI.BackEndApi.Profiles
{
    public class MeasurementPointProfile:Profile
    {
        public MeasurementPointProfile()
        {
            CreateMap<Models.MeasurementPoint, DTO.MeasurementPoint>();
            CreateMap<DTO.MeasurementPoint, Models.MeasurementPoint>();
        }
    }
}