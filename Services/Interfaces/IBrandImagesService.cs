using ZamZamAPI.Dtos.BrandImage;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IBrandImagesService
    {
        Task<Result<List<BrandImageResponseDto>>> GetAll();
        Task<Result<BrandImageResponseDto>> GetById(long id);
        Task<Result<long>> Create(BrandImageRequestDto dto);
        Task<Result<bool>> Update(BrandImageRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
