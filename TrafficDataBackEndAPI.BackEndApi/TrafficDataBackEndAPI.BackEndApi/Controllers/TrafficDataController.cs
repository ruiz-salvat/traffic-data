using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;

namespace TrafficDataBackEndAPI.BackEndApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrafficDataController:ControllerBase
    {
        private const string nullDtoMessage = "Error: null TrafficData input";  
        private const string datetimeError = "End date time smaller than start date time";
        private const string datetimeParseError = "Error with the date format";
        private const string getMPError = "Error getting the MeasurementPoint of the TrafficData. Requested id: ";
        private const string badInputFormatError = "Bad input format";
        private readonly ITrafficDataService trafficDataService;
        private readonly IMeasurementPointService measurementPointService;
        private readonly IMapper mapper;
        private readonly ILogger<TrafficDataController> logger;

        public TrafficDataController(ITrafficDataService trafficDataService, IMeasurementPointService measurementPointService, IMapper mapper, ILogger<TrafficDataController> logger)
        {
            this.trafficDataService = trafficDataService;
            this.measurementPointService = measurementPointService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrafficData()
        {
            IEnumerable<Models.MeasurementPoint> measurementPoints = await measurementPointService.GetMeasurementPoints();
            List<Tuple<Models.TrafficData, Models.MeasurementPoint>> tuples = new List<Tuple<Models.TrafficData, Models.MeasurementPoint>>();
            foreach (var measurementPoint in measurementPoints)
            {
                foreach (var trafficDataRow in measurementPoint.TrafficData)
                {
                    tuples.Add(new Tuple<Models.TrafficData, Models.MeasurementPoint>
                        (trafficDataRow,
                        measurementPoint)
                    );
                }
            }
            return Ok(mapper.Map<IEnumerable<DTO.TrafficData>>(tuples));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrafficDataByMeasurementPointId(int id)
        {
            try 
            {
                List<Tuple<Models.TrafficData, Models.MeasurementPoint>> tuples = new List<Tuple<Models.TrafficData, Models.MeasurementPoint>>();
                Models.MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(id);
                foreach (var trafficDataRow in measurementPoint.TrafficData)
                {
                    tuples.Add(new Tuple<Models.TrafficData, Models.MeasurementPoint>
                        (trafficDataRow,
                        measurementPoint)
                    );
                }
                return Ok(mapper.Map<IEnumerable<DTO.TrafficData>>(tuples));
            } 
            catch (Exception e) 
            {
                string message = getMPError + id;
                logger.LogError($"{message}\r\n{e.Message}", e);
                return NotFound(message);
            }
        }


        [HttpGet("{startDateTime}/{endDateTime}")]
        public async Task<IActionResult> GetTrafficDataTimeInterval(string startDateTime, string endDateTime)
        {
            try 
            {
                DateTime startDateTimeObj = DateTime.Parse(startDateTime);
                DateTime endDateTimeObj = DateTime.Parse(endDateTime);

                if (endDateTimeObj.CompareTo(startDateTimeObj) > 0)
                {
                    IEnumerable<Models.MeasurementPoint> measurementPoints = await measurementPointService.GetMeasurementPoints();
                    List<Tuple<Models.TrafficData, Models.MeasurementPoint>> tuples = new List<Tuple<Models.TrafficData, Models.MeasurementPoint>>();
                    foreach (var measurementPoint in measurementPoints)
                    {
                        foreach (var trafficDataRow in measurementPoint.TrafficData)
                        {
                            if (trafficDataRow.DateTime.CompareTo(startDateTimeObj) > 0 && trafficDataRow.DateTime.CompareTo(endDateTimeObj) < 0)
                            {
                                tuples.Add(new Tuple<Models.TrafficData, Models.MeasurementPoint>
                                    (trafficDataRow,
                                    measurementPoint)
                                );
                            }
                            
                        }
                    }
                    return Ok(mapper.Map<IEnumerable<DTO.TrafficData>>(tuples));
                }
                else
                {
                    return BadRequest(datetimeError);
                }
            } 
            catch (Exception e) 
            {
                logger.LogError($"{datetimeParseError}\r\n{e.Message}", e);
                return BadRequest(datetimeParseError);
            }
        }

        [HttpGet("{id}/{startDateTime}/{endDateTime}")]
        public async Task<IActionResult> GetTrafficDataTimeIntervalByMeasurementPointId(int id, string startDateTime, string endDateTime)
        {
            try 
            {
                DateTime startDateTimeObj = DateTime.Parse(startDateTime);
                DateTime endDateTimeObj = DateTime.Parse(endDateTime);

                if (endDateTimeObj.CompareTo(startDateTimeObj) > 0)
                {
                    try 
                    {
                        List<Tuple<Models.TrafficData, Models.MeasurementPoint>> tuples = new List<Tuple<Models.TrafficData, Models.MeasurementPoint>>();
                        Models.MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(id);
                        foreach (var trafficDataRow in measurementPoint.TrafficData)
                        {
                            if (trafficDataRow.DateTime.CompareTo(startDateTimeObj) > 0 && trafficDataRow.DateTime.CompareTo(endDateTimeObj) < 0)
                            {
                                tuples.Add(new Tuple<Models.TrafficData, Models.MeasurementPoint>
                                    (trafficDataRow,
                                    measurementPoint)
                                );
                            }
                        }
                        return Ok(mapper.Map<IEnumerable<DTO.TrafficData>>(tuples));
                    } 
                    catch (Exception e) 
                    {
                        string message = getMPError + id;
                        logger.LogError($"{message}\r\n{e.Message}", e);
                        return NotFound(message);
                    }
                }
                else
                {
                    return BadRequest(datetimeError);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"{datetimeParseError}\r\n{e.Message}", e);
                return BadRequest(datetimeParseError);
            }
        }

        [HttpPost] 
        public async Task<IActionResult> AddTrafficDataEndPoint([FromBody] Object jObject)
        {
            if (jObject == null)
            {
                return BadRequest(nullDtoMessage);
            }

            try
            {
                IEnumerable<DTO.TrafficData> trafficDataDto = JsonConvert.DeserializeObject<IEnumerable<DTO.TrafficData>>(jObject.ToString());
                List<DTO.TrafficData> trafficDataDtoFromAdd = (List<DTO.TrafficData>)await AddTrafficData(trafficDataDto);
                if (trafficDataDtoFromAdd.Count > 0)
                    return Ok(trafficDataDtoFromAdd);
                else 
                    return BadRequest("Error adding traffic data. No rows could be added.");
            } 
            catch (Exception e1)
            {
                try 
                {
                    DTO.TrafficData trafficDataRowDto = JsonConvert.DeserializeObject<DTO.TrafficData>(jObject.ToString());
                    DTO.TrafficData trafficDataRowDtoFromAdd = await AddTrafficDataRow(trafficDataRowDto);
                    if (trafficDataRowDtoFromAdd != null)
                        return Ok(trafficDataRowDtoFromAdd);
                    else
                        return BadRequest(getMPError + trafficDataRowDto.MeasurementPointId);
                } 
                catch (Exception e2)
                {
                    Console.WriteLine(e1.StackTrace + "\n" + e2.StackTrace);
                    return BadRequest(badInputFormatError);
                }
            }
        }

        private async Task<DTO.TrafficData> AddTrafficDataRow(DTO.TrafficData trafficDataRowDto)
        {
            Models.TrafficData trafficDataRow = mapper.Map<Models.TrafficData>(trafficDataRowDto);

            Models.MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(trafficDataRowDto.MeasurementPointId);
            if (measurementPoint == null)
                return null;

            await trafficDataService.AddTrafficDataRow(trafficDataRow);

            List<Models.TrafficData> measurementPointTrafficDataList = (List<Models.TrafficData>)measurementPoint.TrafficData;
            measurementPointTrafficDataList.Add(trafficDataRow);
            await measurementPointService.UpdateMeasurementPoint(measurementPoint);

            return mapper.Map<DTO.TrafficData>(new Tuple<Models.TrafficData, Models.MeasurementPoint>(trafficDataRow, measurementPoint));
        }

        private async Task<IEnumerable<DTO.TrafficData>> AddTrafficData(IEnumerable<DTO.TrafficData> trafficDataDto)
        {
            List<DTO.TrafficData> trafficDataDtoFromAdd = new List<DTO.TrafficData>();
            foreach (var trafficDataRowDto in trafficDataDto)
            {
                trafficDataDtoFromAdd.Add(await AddTrafficDataRow(trafficDataRowDto));
            }

            return trafficDataDtoFromAdd;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTrafficDataEndPoint([FromBody] Object jObject)
        {
            if (jObject == null)
            {
                return BadRequest(nullDtoMessage);
            }

            try
            {
                IEnumerable<DTO.TrafficData> trafficDataDto = JsonConvert.DeserializeObject<IEnumerable<DTO.TrafficData>>(jObject.ToString());
                List<DTO.TrafficData> trafficDataDtoFromUpdate = (List<DTO.TrafficData>)await UpdateTrafficData(trafficDataDto);
                if (trafficDataDtoFromUpdate.Count > 0)
                    return Ok(trafficDataDtoFromUpdate);
                else
                    return NotFound("Error updating traffic data. No rows found.");
            } 
            catch (Exception e1)
            {
                try 
                {
                    DTO.TrafficData trafficDataRowDto = JsonConvert.DeserializeObject<DTO.TrafficData>(jObject.ToString());
                    DTO.TrafficData trafficDataRowDtoFromUpdate = await UpdateTrafficDataRow(trafficDataRowDto);
                    if (trafficDataRowDtoFromUpdate != null)
                        return Ok(trafficDataRowDtoFromUpdate);
                    else
                        return NotFound("Error updating the traffic data row. Row not found.");
                } 
                catch (Exception e2)
                {
                    Console.WriteLine(e1.StackTrace + "\n" + e2.StackTrace);
                    return BadRequest(badInputFormatError);
                }
            }
        }

        private async Task<DTO.TrafficData> UpdateTrafficDataRow(DTO.TrafficData trafficDataRowDto)
        {
            List<Models.TrafficData> trafficDataDB = (List<Models.TrafficData>)await 
                trafficDataService.GetTrafficDataTimeIntervalByMeasurementPointId(trafficDataRowDto.MeasurementPointId, trafficDataRowDto.DateTime, trafficDataRowDto.DateTime);
            
            // The list should just contain 1 row of traffic data (start and end date at GetTrafficDataTimeIntervalByMeasurementPointId are equal)
            if (trafficDataDB.Count == 1)
            {
                Models.TrafficData trafficDataRowDB = trafficDataDB.SingleOrDefault();

                // Updates just apply to flow and speed
                trafficDataRowDB.Flow = trafficDataRowDto.Flow;
                trafficDataRowDB.Speed = trafficDataRowDto.Speed;
                
                Models.TrafficData trafficDataRow = await trafficDataService.UpdateTrafficDataRow(trafficDataRowDB);
                Models.MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(trafficDataRowDto.MeasurementPointId);
                Tuple<Models.TrafficData, Models.MeasurementPoint> tuple = new Tuple<Models.TrafficData, Models.MeasurementPoint>(trafficDataRow, measurementPoint);
                return mapper.Map<DTO.TrafficData>(tuple);
            }
            return null;
        }

        private async Task<IEnumerable<DTO.TrafficData>> UpdateTrafficData([FromBody] IEnumerable<DTO.TrafficData> trafficDataDto)
        {
            List<DTO.TrafficData> trafficDataDtoList = new List<DTO.TrafficData>();
            foreach (var trafficDataRowDto in trafficDataDto)
            {
                DTO.TrafficData trafficDataRowDtoFromUpdate = await UpdateTrafficDataRow(trafficDataRowDto);
                if (trafficDataRowDtoFromUpdate != null)
                    trafficDataDtoList.Add(trafficDataRowDtoFromUpdate);
            }
            return trafficDataDtoList;
        }

        [HttpDelete("{startDateTime}/{endDateTime}")]
        public async Task<IActionResult> DeleteTrafficDataTimeInterval(string startDateTime, string endDateTime)
        {
            try 
            {
                DateTime startDateTimeObj = DateTime.Parse(startDateTime);
                DateTime endDateTimeObj = DateTime.Parse(endDateTime);

                if (endDateTimeObj.CompareTo(startDateTimeObj) > 0)
                {
                    IEnumerable<Models.TrafficData> trafficData = 
                        await trafficDataService.GetTrafficDataTimeInterval(startDateTimeObj, endDateTimeObj);

                    List<int> ids = new List<int>();
                    foreach (var trafficDataRow in trafficData)
                    {
                        ids.Add(trafficDataRow.Id);
                    }

                    await trafficDataService.DeleteTrafficData(ids);
                    return Ok();
                }
                else
                {
                    return BadRequest(datetimeError);
                }
            }
            catch(Exception e)
            {
                logger.LogError($"{datetimeParseError}\r\n{e.Message}", e);
                return BadRequest(datetimeParseError);
            }
        }

        [HttpDelete("{id}/{startDateTime}/{endDateTime}")]
        public async Task<IActionResult> DeleteTrafficDataTimeIntervalByMeasurementPointId(int id, string startDateTime, string endDateTime)
        {
            try 
            {
                DateTime startDateTimeObj = DateTime.Parse(startDateTime);
                DateTime endDateTimeObj = DateTime.Parse(endDateTime);

                if (endDateTimeObj.CompareTo(startDateTimeObj) > 0)
                {
                    try 
                    {
                        IEnumerable<Models.TrafficData> trafficData = 
                            await trafficDataService.GetTrafficDataTimeIntervalByMeasurementPointId(id, startDateTimeObj, endDateTimeObj);
                        if (trafficData.IsNullOrEmpty())
                            return NotFound(getMPError + id);

                        List<int> ids = new List<int>();
                        foreach (var trafficDataRow in trafficData)
                        {
                            ids.Add(trafficDataRow.Id);
                        }

                        await trafficDataService.DeleteTrafficData(ids);
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        string message = getMPError + id;
                        logger.LogError($"{message}\r\n{e.Message}", e);
                        return NotFound(message);
                    }
                }
                else
                {
                    return BadRequest(datetimeError);
                }
            }
            catch(Exception e)
            {
                logger.LogError($"{datetimeParseError}\r\n{e.Message}", e);
                return BadRequest(datetimeParseError);
            }
        }
    }
}