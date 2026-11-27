using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.SecurityManu;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityManuController(ISecurityManuService securityManuService) : ControllerBase
    {
        private readonly ISecurityManuService securityManuService = securityManuService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] SecurityManuSearchDto searchDto)
        {
            var result = await securityManuService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await securityManuService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SecurityManuRequestDto dto)
        {
            var result = await securityManuService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SecurityManuRequestDto dto)
        {
            var result = await securityManuService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await securityManuService.Delete(id);
            return Ok(result);
        }
    }
}
