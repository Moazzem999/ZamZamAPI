using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.Service;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController(IServicesService servicesService) : ControllerBase
    {
        private readonly IServicesService servicesService = servicesService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ServiceSearchDto searchDto)
        {
            var result = await servicesService.GetAll(searchDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllForDisplay")]
        public async Task<IActionResult> GetAllForDisplay()
        {
            var result = await servicesService.GetAllForDisplay();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await servicesService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ServiceRequestDto dto)
        {
            var result = await servicesService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] ServiceRequestDto dto)
        {
            var result = await servicesService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await servicesService.Delete(id);
            return Ok(result);
        }
    }
}

