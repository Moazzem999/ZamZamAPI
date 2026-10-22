using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.QuotationRequest;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationRequestController(IQuotationRequestService quotationRequestService) : ControllerBase
    {
        private readonly IQuotationRequestService quotationRequestService = quotationRequestService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] QuotationRequestSearchDto searchDto)
        {
            var result = await quotationRequestService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await quotationRequestService.GetById(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuotationRequestRequestDto dto)
        {
            var result = await quotationRequestService.Create(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await quotationRequestService.Delete(id);
            return Ok(result);
        }
    }
}
