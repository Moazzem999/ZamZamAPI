using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.ContactInquiry;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInquiryController(IContactInquiryService contactInquiryService) : ControllerBase
    {
        private readonly IContactInquiryService contactInquiryService = contactInquiryService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ContactInquirySearchDto searchDto)
        {
            var result = await contactInquiryService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await contactInquiryService.GetById(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactInquiryRequestDto dto)
        {
            var result = await contactInquiryService.Create(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await contactInquiryService.Delete(id);
            return Ok(result);
        }
    }
}

