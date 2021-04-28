using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrafficDataBackEndAPI.BackEndApi.Extraction.Pattern;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Controllers
{
    // This controller is used to controll the processes responsible for the data extraction
    [ApiController]
    [Route("api/[controller]")]
    public class DataExtractionController:ControllerBase
    {
        public const string startThreadTag = "StartThread";
        public const string stopThreadTag = "StopThread";
        private readonly DataEngine dataEngine;
        
        public DataExtractionController(DataEngine dataEngine)
        {
            this.dataEngine = dataEngine;
        }

        [HttpGet]
        public IActionResult GetState()
        {
            return Ok(State.ConvertToString(dataEngine.GetState()));
        }

        // Inserts the measurement points to the database (just done once)
        [HttpPost]
        public async Task<IActionResult> RetrieveMeasurementPointsAsync()
        {
            await dataEngine.InsertMeasurementPointsToDBAsync();
            return Ok("Measurement points retrieved correctly");
        }
        
        // Updates traffic data
        [HttpPut]
        public async Task<IActionResult> DoActionAsync([FromBody] Object jObject)
        {
            if (jObject == null)
                return BadRequest("Null action request");

            try
            {
                UserAction userAction = JsonConvert.DeserializeObject<UserAction>(jObject.ToString());
                if (userAction.Tag.Equals(startThreadTag))
                {
                    if (dataEngine.GetState().Equals(State.MeasurementPointsRetrieved) || dataEngine.GetState().Equals(State.Aborted))
                    {
                        // If the state is resumed, it isn't posible to start or stop the application
                        await dataEngine.SetStateAsync(State.Resumed);
                        await dataEngine.RunAsync();
                        return Ok("Traffic data retrievement process terminated");
                    }
                    else
                        return BadRequest("Process is already started or the measurement points are not retrieved yet");
                }
                else if (userAction.Tag.Equals(stopThreadTag))
                {
                    if (dataEngine.GetState().Equals(State.ExtractingTrafficData) || dataEngine.GetState().Equals(State.Sleeping))
                    {
                        // If the state is aborting, it isn't posible to start or stop the application
                        await dataEngine.SetStateAsync(State.Aborting);
                        return Ok("Process is now being aborted");
                    }
                    else
                        return BadRequest("Process is already terminated or not started yet");
                }
                else
                    return BadRequest("Error reading the action request");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return BadRequest("Bad input format");
            }
        }
    }
}