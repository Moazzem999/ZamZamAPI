using ZamZamAPI.Dtos.User;
using ZamZamAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ZamZamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUsersRepository usersRepository) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var data = await usersRepository.Login(dto);
            return Ok(data);
        }
    }
}
