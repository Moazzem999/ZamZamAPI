using ZamZamAPI.Dtos.EmployeeTask;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IEmployeeTaskService
    {
        Task<Result<PagedResult<EmployeeTaskResponseDto>>> GetAll(EmployeeTaskSearchDto searchDto);
        Task<Result<EmployeeTaskResponseDto>> GetById(long id);
        Task<Result<long>> Create(EmployeeTaskRequestDto dto);
        Task<Result<EmployeeTaskResponseDto>> Update(EmployeeTaskRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
