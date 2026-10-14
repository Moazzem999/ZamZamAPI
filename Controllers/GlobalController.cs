using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalController(IBrandImagesService brandImagesService, IAboutUsService aboutUsService) : ControllerBase
    {
        [HttpGet("GetAllBrandImages")]
        public async Task<IActionResult> GetAllBrandImages()
        {
            var result = await brandImagesService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetActiveAboutUs")]
        public async Task<IActionResult> GetActiveAboutUs()
        {
            var result = await aboutUsService.GetActiveAboutUs();
            return Ok(result);
        }
    }
}
