using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.LocationHistory;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalController(IBrandImagesService brandImagesService
        , IAboutUsService aboutUsService
        , ILocationHistoryService locationHistoryService
        , IDropdownService dropdownService) : ControllerBase
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

        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] LocationHistoryRequestDto dto)
        {
            var result = await locationHistoryService.Create(dto);
            return Ok(result);
        }

        [HttpGet("GetAllServiceCategory")]
        public async Task<IActionResult> GetAllServiceCategory()
        {
            var result = await dropdownService.GetAllServiceCategory();
            return Ok(result);
        }
    }
}
