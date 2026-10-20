using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.LocationHistory;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationHistoryController(ILocationHistoryService locationHistoryService) : ControllerBase
    {
        private readonly ILocationHistoryService locationHistoryService = locationHistoryService;

        [HttpGet("GetAll/{employeeId}")]
        public async Task<IActionResult> GetAll([FromQuery] LocationHistorySearchDto searchDto)
        {
            var result = await locationHistoryService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await locationHistoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LocationHistoryRequestDto dto)
        {
            var result = await locationHistoryService.Create(dto);
            return Ok(result);
        }
    }
}
