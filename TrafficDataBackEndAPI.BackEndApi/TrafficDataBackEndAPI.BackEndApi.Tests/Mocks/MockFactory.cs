using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrafficDataBackEndAPI.BackEndApi.Models;

namespace TrafficDataBackEndAPI.BackEndApi.Tests.Mocks
{
    public class MockFactory 
    {
        // This method returns a list of ids from 1 to 'size' incrementing by 1 each element
        public static IEnumerable<int> CreateIdList(int size)
        {
            if (size > 1)
            {
                List<int> list = new List<int>();
                for (int i = 1; i <= size; i++)
                {
                    list.Add(i);
                }
                return list;
            } else {
                throw new ArgumentException();
            }
        }

        public static DTO.MeasurementPoint CreateMeasurementPointDto()
        {
            return new DTO.MeasurementPoint
            {
                Reference = "Waterlinieweg",
                Latitude = 52.084241,
                Longitude = 5.146836
            };
        }

        public static Models.MeasurementPoint CreateMeasurementPoint()
        {
            return new Models.MeasurementPoint
            {
                Reference = "Waterlinieweg",
                Latitude = 52.084241,
                Longitude = 5.146836,
                TrafficData = CreateTrafficDataList()
            };
        }

        public static async Task<Models.MeasurementPoint> CreateMeasurementPointAsync()
        {
            return await Task.Run(async () => new Models.MeasurementPoint
            {
                Reference = "Waterlinieweg",
                Latitude = 52.084241,
                Longitude = 5.146836,
                TrafficData = await CreateTrafficDataListAsync()
            });
        }

        public static IEnumerable<Models.MeasurementPoint> CreateMeasurementPointList()
        {
            return new List<Models.MeasurementPoint> {
                new Models.MeasurementPoint 
                { 
                    Reference = "A9",
                    Latitude = 52.294608,
                    Longitude = 4.891013,
                    TrafficData = CreateTrafficDataList()
                },
                new Models.MeasurementPoint
                {
                    Reference = "A6",
                    Latitude = 52.331500,
                    Longitude = 5.150339,
                    TrafficData = CreateTrafficDataList()
                },
                new Models.MeasurementPoint
                {
                    Reference = "A200",
                    Latitude = 52.386483,
                    Longitude = 4.738732,
                    TrafficData = CreateTrafficDataList()
                },
                new Models.MeasurementPoint
                {
                    Reference = "A5",
                    Latitude = 52.367273,
                    Longitude = 4.754335,
                    TrafficData = CreateTrafficDataList()
                }
            };
        }

        public static async Task<IEnumerable<Models.MeasurementPoint>> CreateMeasurementPointListAsync()
        { 
            return await Task.Run(async () => new List<Models.MeasurementPoint> {
                new Models.MeasurementPoint 
                { 
                    Reference = "A9",
                    Latitude = 52.294608,
                    Longitude = 4.891013,
                    TrafficData = await CreateTrafficDataListAsync()
                },
                new Models.MeasurementPoint
                {
                    Reference = "A6",
                    Latitude = 52.331500,
                    Longitude = 5.150339,
                    TrafficData = await CreateTrafficDataListAsync()
                },
                new Models.MeasurementPoint
                {
                    Reference = "A200",
                    Latitude = 52.386483,
                    Longitude = 4.738732,
                    TrafficData = await CreateTrafficDataListAsync()
                },
                new Models.MeasurementPoint
                {
                    Reference = "A5",
                    Latitude = 52.367273,
                    Longitude = 4.754335,
                    TrafficData = await CreateTrafficDataListAsync()
                }
            });
        }

        public static IEnumerable<Models.TrafficData> CreateTrafficDataList()
        {
            return new List<Models.TrafficData> {
                new Models.TrafficData {
                    DateTime = Constants.MockDatetime,
                    Flow = 500,
                    Speed = 100
                },
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 600,
                    Speed = 110
                }, 
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 700,
                    Speed = 90
                },
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 800,
                    Speed = 105
                },
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 900,
                    Speed = 89
                }
            };
        }

        public static IEnumerable<Models.TrafficData> CreateTrafficDataListWithIds()
        {
            return new List<Models.TrafficData> {
                new Models.TrafficData {
                    Id = 1,
                    DateTime = Constants.MockDatetime,
                    Flow = 500,
                    Speed = 100
                },
                new Models.TrafficData {
                    Id = 2,
                    DateTime = DateTime.Now,
                    Flow = 600,
                    Speed = 110
                },
                new Models.TrafficData {
                    Id = 3,
                    DateTime = DateTime.Now,
                    Flow = 700,
                    Speed = 90
                },
                new Models.TrafficData {
                    Id = 4,
                    DateTime = DateTime.Now,
                    Flow = 800,
                    Speed = 105
                },
                new Models.TrafficData {
                    Id = 5,
                    DateTime = DateTime.Now,
                    Flow = 900,
                    Speed = 89
                }
            };
        }

        public static async Task<IEnumerable<Models.TrafficData>> CreateTrafficDataListAsync()
        {
            return await Task.Run(() => new List<Models.TrafficData> {
                new Models.TrafficData {
                    DateTime = Constants.MockDatetime,
                    Flow = 500,
                    Speed = 100
                },
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 600,
                    Speed = 110
                }, 
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 700,
                    Speed = 90
                },
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 800,
                    Speed = 105
                },
                new Models.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 900,
                    Speed = 89
                }
            });
        }

        public static async Task<IEnumerable<TrafficData>> CreateTrafficDataListSingleRowAsync()
        {
            return await Task.Run(() => new List<TrafficData>
            {
                new TrafficData {
                    DateTime = Constants.MockDatetime,
                    Flow = 999,
                    Speed = 999
                }
            });
        }

        public static TrafficData CreateTrafficDataRow()
        {
            return new TrafficData{
                DateTime = Constants.MockDatetime,
                Flow = 999,
                Speed = 999
            };
        }

        public static async Task<TrafficData> CreateTrafficDataRowAsync()
        {
            return await Task.Run(() => new TrafficData{
                DateTime = Constants.MockDatetime,
                Flow = 999,
                Speed = 999
            });
        }

        public static IEnumerable<DTO.TrafficData> CreateTrafficDataDtoList()
        {
            return new List<DTO.TrafficData> {
                new DTO.TrafficData {
                    DateTime = Constants.MockDatetime,
                    Flow = 500,
                    Speed = 100,
                    MeasurementPointId = 1
                },
                new DTO.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 600,
                    Speed = 110,
                    MeasurementPointId = 2
                }, 
                new DTO.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 700,
                    Speed = 90,
                    MeasurementPointId = 3
                },
                new DTO.TrafficData {
                    DateTime = DateTime.Now,
                    Flow = 800,
                    Speed = 105,
                    MeasurementPointId = 4
                }
            };
        }

        public static string CreateTrafficDataRowStringDto()
        {
            return "{\"dateTime\": \"2019-12-01T22:00:00\",\"flow\": 180,\"speed\": 90,\"measurementPointId\": 1}";
        }

        public static string CreateTrafficDataListStringDto()
        {
            return "[{ \"dateTime\": \"2019-12-01T22:00:00\",\"flow\": 180,\"speed\": 90,\"measurementPointId\": 1}]";
        }

        public static string CreateStartExtractionRequestDto()
        {
            return "{\"Tag\":\"StartThread\"}";
        }

        public static string CreateStopExtractionRequestDto()
        {
            return "{\"Tag\":\"StopThread\"}";
        }
    }
}