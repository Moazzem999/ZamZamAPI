using ZamZamAPI.Dtos.LocationHistory;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface ILocationHistoryRepository
    {
        Task<Result<PagedResult<LocationHistoryResponseDto>>> GetAll(LocationHistorySearchDto searchDto);
        Task<Result<LocationHistoryResponseDto>> GetById(long id);
        Task<Result<long>> Create(LocationHistoryRequestDto dto);
    }
}
