using ZamZamAPI.Dtos.MachineryCategory;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IMachineryCategoryService
    {
        Task<Result<PagedResult<MachineryCategoryResponseDto>>> GetAll(MachineryCategorySearchDto searchDto);
        Task<Result<List<MachineryCategoryResponseDto>>> GetAllForDisplay();
        Task<Result<MachineryCategoryResponseDto>> GetById(long id);
        Task<Result<long>> Create(MachineryCategoryRequestDto dto);
        Task<Result<MachineryCategoryResponseDto>> Update(MachineryCategoryRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}

