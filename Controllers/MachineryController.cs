using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.Machinery;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineryController(IMachineryService machineryService) : ControllerBase
    {
        private readonly IMachineryService machineryService = machineryService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] MachinerySearchDto searchDto)
        {
            var result = await machineryService.GetAll(searchDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllForDisplay/{categoryId:long?}")]
        public async Task<IActionResult> GetAllForDisplay(long? categoryId)
        {
            var result = await machineryService.GetAllForDisplay(categoryId ?? 0);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await machineryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MachineryRequestDto dto)
        {
            var result = await machineryService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] MachineryRequestDto dto)
        {
            var result = await machineryService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await machineryService.Delete(id);
            return Ok(result);
        }
    }
}

