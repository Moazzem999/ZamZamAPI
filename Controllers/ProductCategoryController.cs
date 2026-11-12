using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.ProductCategory;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController(IProductCategoryService productCategoryService) : ControllerBase
    {
        private readonly IProductCategoryService productCategoryService = productCategoryService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductCategorySearchDto searchDto)
        {
            var result = await productCategoryService.GetAll(searchDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllForDisplay")]
        public async Task<IActionResult> GetAllForDisplay()
        {
            var result = await productCategoryService.GetAllForDisplay();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await productCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await productCategoryService.GetByName(name);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCategoryRequestDto dto)
        {
            var result = await productCategoryService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductCategoryRequestDto dto)
        {
            var result = await productCategoryService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await productCategoryService.Delete(id);
            return Ok(result);
        }
    }
}

