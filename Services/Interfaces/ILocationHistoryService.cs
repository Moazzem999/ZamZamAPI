using ZamZamAPI.Dtos.LocationHistory;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface ILocationHistoryService
    {
        Task<Result<PagedResult<LocationHistoryResponseDto>>> GetAll(LocationHistorySearchDto searchDto);
        Task<Result<LocationHistoryResponseDto>> GetById(long id);
        Task<Result<long>> Create(LocationHistoryRequestDto dto);
    }
}
