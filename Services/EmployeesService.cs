using ZamZamAPI.Dtos.Employee;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class EmployeesService(IEmployeesRepository employeesRepository) : IEmployeesService
    {
        private readonly IEmployeesRepository employeesRepository = employeesRepository;

        public async Task<Result<PagedResult<EmployeeResponseDto>>> GetAll(EmployeeSearchDto searchDto)
        {
            return await employeesRepository.GetAll(searchDto);
        }

        public async Task<Result<EmployeeResponseDto>> GetById(long id)
        {
            return await employeesRepository.GetById(id);
        }

        public async Task<Result<List<EmployeeResponseDto>>> GetByName(string name)
        {
            return await employeesRepository.GetByName(name);
        }

        public async Task<Result<long>> Create(EmployeeRequestDto dto)
        {
            return await employeesRepository.Create(dto);
        }

        public async Task<Result<EmployeeResponseDto>> Update(EmployeeRequestDto dto)
        {
            return await employeesRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await employeesRepository.Delete(id);
        }
    }
}
