using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZamZamAPI.Dtos.EmployeeTask;
using ZamZamAPI.Services.Interfaces;

namespace ZamZamAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTaskController(IEmployeeTaskService employeeTaskService) : ControllerBase
    {
        private readonly IEmployeeTaskService employeeTaskService = employeeTaskService;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] EmployeeTaskSearchDto searchDto)
        {
            var result = await employeeTaskService.GetAll(searchDto);
            return Ok(result);
        }

        [HttpGet("GetAllByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetAllByEmployeeId(long employeeId, [FromQuery] EmployeeTaskSearchDto searchDto)
        {
            var result = await employeeTaskService.GetAllByEmployeeId(employeeId, searchDto);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await employeeTaskService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeTaskRequestDto dto)
        {
            var result = await employeeTaskService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EmployeeTaskRequestDto dto)
        {
            var result = await employeeTaskService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await employeeTaskService.Delete(id);
            return Ok(result);
        }
    }
}
