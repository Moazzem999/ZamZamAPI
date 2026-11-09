using ZamZamAPI.Dtos.Service;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IServicesRepository
    {
        Task<Result<PagedResult<ServiceResponseDto>>> GetAll(ServiceSearchDto searchDto);
        Task<Result<ServiceResponseDto>> GetById(long id);
        Task<Result<long>> Create(ServiceRequestDto dto);
        Task<Result<ServiceResponseDto>> Update(ServiceRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

