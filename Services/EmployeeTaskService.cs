using ZamZamAPI.Dtos.EmployeeTask;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class EmployeeTaskService(IEmployeeTaskRepository employeeTaskRepository) : IEmployeeTaskService
    {
        private readonly IEmployeeTaskRepository employeeTaskRepository = employeeTaskRepository;

        public async Task<Result<PagedResult<EmployeeTaskResponseDto>>> GetAll(EmployeeTaskSearchDto searchDto)
        {
            return await employeeTaskRepository.GetAll(searchDto);
        }

        public async Task<Result<PagedResult<EmployeeTaskResponseDto>>> GetAllByEmployeeId(long employeeId, EmployeeTaskSearchDto searchDto)
        {
            return await employeeTaskRepository.GetAllByEmployeeId(employeeId, searchDto);
        }

        public async Task<Result<EmployeeTaskResponseDto>> GetById(long id)
        {
            return await employeeTaskRepository.GetById(id);
        }

        public async Task<Result<long>> Create(EmployeeTaskRequestDto dto)
        {
            return await employeeTaskRepository.Create(dto);
        }

        public async Task<Result<EmployeeTaskResponseDto>> Update(EmployeeTaskRequestDto dto)
        {
            return await employeeTaskRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await employeeTaskRepository.Delete(id);
        }
    }
}
