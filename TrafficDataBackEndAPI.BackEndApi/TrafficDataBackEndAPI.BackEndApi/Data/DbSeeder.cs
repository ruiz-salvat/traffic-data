using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TrafficDataBackEndAPI.BackEndApi.Models;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Data
{
    public class DbSeeder
    {
        private readonly Context context;

        public DbSeeder(Context context)
        {
            this.context = context;
        }

        public void Seed()
        {
            if (context.MeasurementPoints.Any())
                // DB has been seeded
                return; 

            var dataSets = new Dictionary<int, List<TrafficData>>();

            for (int file_num = 1; file_num <= 5; file_num++)
            {
                using (var reader = new StreamReader("Data//Files//data_" + file_num + ".csv"))
                {
                    List<TrafficData> trafficData = new List<TrafficData>();
                    List<string> dateTimeList = new List<string>();
                    List<string> flowList = new List<string>();
                    List<string> speedList = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        dateTimeList.Add(values[0]);
                        flowList.Add(values[1]);
                        speedList.Add(values[2]);
                    }
                    for (int i = 1; i < dateTimeList.Count; i++) // First element is 1 to avoid the header of the csv file
                    {
                        trafficData.Add(
                            new TrafficData
                            {
                                DateTime = DateTime.ParseExact(dateTimeList[i], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                Flow = (int)double.Parse(flowList[i]),
                                Speed = double.Parse(speedList[i])
                            }
                        );
                    }
                    dataSets[file_num] = trafficData;
                }
            }

            List<MeasurementPoint> measurementPoints = new List<MeasurementPoint> 
            {
                new MeasurementPoint 
                {
                    Reference = "seed_data_1",
                    Latitude = 52.136197,
                    Longitude = 5.008255,
                    TrafficData = dataSets[1]
                },
                new MeasurementPoint
                {
                    Reference = "seed_data_2",
                    Latitude = 52.132560,
                    Longitude = 5.152065,
                    TrafficData = dataSets[2]
                },
                new MeasurementPoint
                {
                    Reference = "seed_data_3",
                    Latitude = 52.071315,
                    Longitude = 5.040024,
                    TrafficData = dataSets[3]
                },
                new MeasurementPoint
                {
                    Reference = "seed_data_4",
                    Latitude = 52.100801,
                    Longitude = 5.212987,
                    TrafficData = dataSets[4]
                },
                new MeasurementPoint
                {
                    Reference = "seed_data_5",
                    Latitude = 52.134029,
                    Longitude = 5.086433,
                    TrafficData = dataSets[5]
                }
            };

            context.AddRange(dataSets[1]);
            context.AddRange(dataSets[2]);
            context.AddRange(dataSets[3]);
            context.AddRange(dataSets[4]);
            context.AddRange(dataSets[5]);
            context.AddRange(measurementPoints);
            context.Add(new Metadata{
                // Default state is "measurement points not retrieved"
                State = State.MeasurementPointsNotRetrieved
            });
            context.SaveChanges();
        }
    }
}