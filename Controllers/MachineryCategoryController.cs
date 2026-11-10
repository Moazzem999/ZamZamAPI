using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.MachineryCategory;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineryCategoryController(IMachineryCategoryService machineryCategoryService) : ControllerBase
    {
        private readonly IMachineryCategoryService machineryCategoryService = machineryCategoryService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] MachineryCategorySearchDto searchDto)
        {
            var result = await machineryCategoryService.GetAll(searchDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllForDisplay")]
        public async Task<IActionResult> GetAllForDisplay()
        {
            var result = await machineryCategoryService.GetAllForDisplay();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await machineryCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await machineryCategoryService.GetByName(name);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MachineryCategoryRequestDto dto)
        {
            var result = await machineryCategoryService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MachineryCategoryRequestDto dto)
        {
            var result = await machineryCategoryService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await machineryCategoryService.Delete(id);
            return Ok(result);
        }
    }
}

