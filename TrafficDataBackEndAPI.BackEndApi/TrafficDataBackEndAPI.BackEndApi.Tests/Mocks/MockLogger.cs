using Microsoft.Extensions.Logging;
using Moq;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks
{
    public class MockLogger<T>
    {
        public ILogger<T> CreateLogger()
        {
            var mock = new Mock<ILogger<T>>();
            ILogger<T> logger = mock.Object;

            return logger;
        }
    }
}
