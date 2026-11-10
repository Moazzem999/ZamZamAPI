using ZamZamAPI.Dtos.Machinery;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IMachineryRepository
    {
        Task<Result<PagedResult<MachineryResponseDto>>> GetAll(MachinerySearchDto searchDto);
        Task<Result<List<MachineryResponseDto>>> GetAllForDisplay(long categoryId);
        Task<Result<MachineryResponseDto>> GetById(long id);
        Task<Result<long>> Create(MachineryRequestDto dto);
        Task<Result<MachineryResponseDto>> Update(MachineryRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

