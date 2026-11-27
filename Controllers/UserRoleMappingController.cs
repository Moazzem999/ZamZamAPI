using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.UserRoleMapping;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleMappingController(IUserRoleMappingService userRoleMappingService) : ControllerBase
    {
        private readonly IUserRoleMappingService userRoleMappingService = userRoleMappingService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] UserRoleMappingSearchDto searchDto)
        {
            var result = await userRoleMappingService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await userRoleMappingService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRoleMappingRequestDto dto)
        {
            var result = await userRoleMappingService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserRoleMappingRequestDto dto)
        {
            var result = await userRoleMappingService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await userRoleMappingService.Delete(id);
            return Ok(result);
        }
    }
}
