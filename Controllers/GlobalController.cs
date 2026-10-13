using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalController(IBrandImagesService brandImagesService) : ControllerBase
    {
        [HttpGet("GetAllBrandImages")]
        public async Task<IActionResult> GetAllBrandImages()
        {
            var result = await brandImagesService.GetAll();
            return Ok(result);
        }
    }
}
