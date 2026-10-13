using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.BrandImage;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BrandImagesController(IBrandImagesService brandImagesService) : ControllerBase
    {
        [HttpGet]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await brandImagesService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await brandImagesService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BrandImageRequestDto dto)
        {
            var result = await brandImagesService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] BrandImageRequestDto dto)
        {
            var result = await brandImagesService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await brandImagesService.Delete(id);
            return Ok(result);
        }
    }
}
