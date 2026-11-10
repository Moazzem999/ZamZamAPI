using ZamZamAPI.Dtos.MachineryCategory;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IMachineryCategoryRepository
    {
        Task<Result<PagedResult<MachineryCategoryResponseDto>>> GetAll(MachineryCategorySearchDto searchDto);
        Task<Result<List<MachineryCategoryResponseDto>>> GetAllForDisplay();
        Task<Result<MachineryCategoryResponseDto>> GetById(long id);
        Task<Result<List<MachineryCategoryResponseDto>>> GetByName(string name);
        Task<Result<long>> Create(MachineryCategoryRequestDto dto);
        Task<Result<MachineryCategoryResponseDto>> Update(MachineryCategoryRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

