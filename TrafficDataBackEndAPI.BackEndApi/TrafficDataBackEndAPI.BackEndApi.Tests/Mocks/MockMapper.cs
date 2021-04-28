using AutoMapper;
using TrafficDataBackEndAPI.BackEndApi.Profiles;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks
{
    public class MockMapper
    {
        public static IMapper CreateMapper()
        {
            var measurementPointProfile = new MeasurementPointProfile();
            var trafficDataProfile = new TrafficDataProfile();
            var configuration = new MapperConfiguration(cfg => {
                    cfg.AddProfile(measurementPointProfile);
                    cfg.AddProfile(trafficDataProfile);
                }
            );

            IMapper mapper = new Mapper(configuration);
            return mapper;
        }
    }
}
