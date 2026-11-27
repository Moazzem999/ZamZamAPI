using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.SecurityRolePermission;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityRolePermissionController(ISecurityRolePermissionService securityRolePermissionService) : ControllerBase
    {
        private readonly ISecurityRolePermissionService securityRolePermissionService = securityRolePermissionService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] SecurityRolePermissionSearchDto searchDto)
        {
            var result = await securityRolePermissionService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<SecurityRolePermissionRequestDto> dtos)
        {
            var result = await securityRolePermissionService.Create(dtos);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] List<SecurityRolePermissionRequestDto> dtos)
        {
            var result = await securityRolePermissionService.Update(dtos);
            return Ok(result);
        }
    }
}
