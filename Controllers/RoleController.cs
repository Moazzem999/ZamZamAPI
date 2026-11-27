using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.Role;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService roleService = roleService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] RoleSearchDto searchDto)
        {
            var result = await roleService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await roleService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await roleService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleRequestDto dto)
        {
            var result = await roleService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RoleRequestDto dto)
        {
            var result = await roleService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await roleService.Delete(id);
            return Ok(result);
        }
    }
}
