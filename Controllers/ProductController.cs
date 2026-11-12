using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.Product;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService productService = productService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ProductSearchDto searchDto)
        {
            var result = await productService.GetAll(searchDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllForDisplay/{categoryId:long?}")]
        public async Task<IActionResult> GetAllForDisplay(long? categoryId)
        {
            var result = await productService.GetAllForDisplay(categoryId ?? 0);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await productService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProductRequestDto dto)
        {
            var result = await productService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] ProductRequestDto dto)
        {
            var result = await productService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await productService.Delete(id);
            return Ok(result);
        }
    }
}

