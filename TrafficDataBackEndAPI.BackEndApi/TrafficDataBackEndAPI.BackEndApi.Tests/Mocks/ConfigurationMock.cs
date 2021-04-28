using Microsoft.Extensions.Configuration;
using Moq;

namespace Extract.TrafficData.Test.Mocks
{
    public static class ConfigurationMock
    {
        public static IConfiguration GetMockConfiguration()
        {
            var configuration = new Mock<IConfiguration>();

            var TDCompressedFilePathConfig = new Mock<IConfigurationSection>();
            TDCompressedFilePathConfig.Setup(a => a.Value).Returns("Extraction/Files/trafficspeed.xml.gz");

            var TDXMLFilePathConfig = new Mock<IConfigurationSection>();
            TDXMLFilePathConfig.Setup(a => a.Value).Returns("Extraction/Files/trafficspeed.xml");

            var MPCompressedFilePathConfig = new Mock<IConfigurationSection>();
            MPCompressedFilePathConfig.Setup(a => a.Value).Returns("Extraction/Files/measurement.xml.gz");

            var MPXMLFilePathConfig = new Mock<IConfigurationSection>();
            MPXMLFilePathConfig.Setup(a => a.Value).Returns("Extraction/Files/measurement.xml");

            var MPURLConfig = new Mock<IConfigurationSection>();
            MPURLConfig.Setup(a => a.Value).Returns("http://opendata.ndw.nu/measurement.xml.gz");

            var TDURLConfig = new Mock<IConfigurationSection>();
            TDURLConfig.Setup(a => a.Value).Returns("http://opendata.ndw.nu/trafficspeed.xml.gz");

            var CoordinateLimitsMaxLatConfig = new Mock<IConfigurationSection>();
            CoordinateLimitsMaxLatConfig.Setup(a => a.Value).Returns("52.161577");

            var CoordinateLimitsMinLatConfig = new Mock<IConfigurationSection>();
            CoordinateLimitsMinLatConfig.Setup(a => a.Value).Returns("51.931903");

            var CoordinateLimitsMaxLonConfig = new Mock<IConfigurationSection>();
            CoordinateLimitsMaxLonConfig.Setup(a => a.Value).Returns("5.169142");

            var CoordinateLimitsMinLonConfig = new Mock<IConfigurationSection>();
            CoordinateLimitsMinLonConfig.Setup(a => a.Value).Returns("4.981977");

            var RefreshingTimeConfig = new Mock<IConfigurationSection>();
            RefreshingTimeConfig.Setup(a => a.Value).Returns("120000");

            var RetentionInDaysConfig = new Mock<IConfigurationSection>();
            RetentionInDaysConfig.Setup(a => a.Value).Returns("3");

            configuration.Setup(a => a.GetSection("FilePaths:TrafficDataCompressedFilePath")).Returns(TDCompressedFilePathConfig.Object);
            configuration.Setup(a => a.GetSection("FilePaths:TrafficDataXMLFilePath")).Returns(TDXMLFilePathConfig.Object);
            configuration.Setup(a => a.GetSection("FilePaths:MeasurementPointCompressedFilePath")).Returns(MPCompressedFilePathConfig.Object);
            configuration.Setup(a => a.GetSection("FilePaths:MeasurementPointXMLFilePath")).Returns(MPXMLFilePathConfig.Object);
            configuration.Setup(a => a.GetSection("URLs:MeasurementPointsURL")).Returns(MPURLConfig.Object);
            configuration.Setup(a => a.GetSection("URLs:TrafficDataURL")).Returns(TDURLConfig.Object);
            configuration.Setup(a => a.GetSection("CoordinateLimits:MaxLat")).Returns(CoordinateLimitsMaxLatConfig.Object);
            configuration.Setup(a => a.GetSection("CoordinateLimits:MinLat")).Returns(CoordinateLimitsMinLatConfig.Object);
            configuration.Setup(a => a.GetSection("CoordinateLimits:MaxLon")).Returns(CoordinateLimitsMaxLonConfig.Object);
            configuration.Setup(a => a.GetSection("CoordinateLimits:MinLon")).Returns(CoordinateLimitsMinLonConfig.Object);
            configuration.Setup(a => a.GetSection("TimeLimits:RefreshingTime")).Returns(RefreshingTimeConfig.Object);
            configuration.Setup(a => a.GetSection("TimeLimits:RetentionInDays")).Returns(RetentionInDaysConfig.Object);

            return configuration.Object;
        }
    }
}
