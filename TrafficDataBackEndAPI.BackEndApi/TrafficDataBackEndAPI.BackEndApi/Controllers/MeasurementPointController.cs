using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrafficDataBackEndAPI.BackEndApi.Interfaces;

namespace TrafficDataBackEndAPI.BackEndApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementPointController:ControllerBase
    {
        private const string NullDtoMessage = "Error: null MeasurementPoint";     
        private readonly IMeasurementPointService measurementPointService;
        private readonly IMapper mapper;
        private readonly ILogger<MeasurementPointController> logger;

        public MeasurementPointController(IMeasurementPointService measurementPointService, IMapper mapper, ILogger<MeasurementPointController> logger)
        {
            this.measurementPointService = measurementPointService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeasurementPoints()
        {
            IEnumerable<Models.MeasurementPoint> measurementPoints = await measurementPointService.GetMeasurementPoints();
            return Ok(mapper.Map<IEnumerable<DTO.MeasurementPoint>>(measurementPoints));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeasurementPoint(int id)
        {
            try 
            {
                Models.MeasurementPoint measurementPoint = await measurementPointService.GetMeasurementPoint(id);
                return Ok(mapper.Map<DTO.MeasurementPoint>(measurementPoint));
            } 
            catch (Exception e) 
            {
                string message = $"Error getting the MeasurementPoint. Requested id: {id}";
                logger.LogError($"{message}\r\n{e.Message}", e);
                return NotFound(message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMeasurementPoint([FromBody] DTO.MeasurementPoint measurementPointDto)
        {
            if (measurementPointDto == null)
                return BadRequest(NullDtoMessage);
            
            Models.MeasurementPoint measurementPoint = await measurementPointService.AddMeasurementPoint(mapper.Map<Models.MeasurementPoint>(measurementPointDto));
            return Ok(mapper.Map<DTO.MeasurementPoint>(measurementPoint));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMeasurementPoint([FromBody] DTO.MeasurementPoint measurementPointDto)
        {
            if (measurementPointDto == null)
                return BadRequest(NullDtoMessage);
            
            try 
            {
                Models.MeasurementPoint measurementPoint = await measurementPointService.UpdateMeasurementPoint(mapper.Map<Models.MeasurementPoint>(measurementPointDto));
                return Ok(mapper.Map<DTO.MeasurementPoint>(measurementPoint));
            } 
            catch (Exception e) 
            {
                string message = $"Error updating the MeasurementPoint. Requested id: {measurementPointDto.Id}";
                logger.LogError($"{message}\r\n{e.Message}", e);
                return NotFound(message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurementPoint(int id)
        {
            try 
            {
                await measurementPointService.DeleteMeasurementPoint(id);
                return Ok(id);
            } 
            catch (Exception e) 
            {
                string message = $"Error deleting the MeasurementPoint. Requested id: {id}";
                logger.LogError($"{message}\r\n{e.Message}", e);
                return NotFound(message);
            }
        }
    }
}