using ZamZamAPI.Dtos.BrandImage;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IBrandImagesRepository
    {
        Task<Result<List<BrandImageResponseDto>>> GetAll();
        Task<Result<BrandImageResponseDto>> GetById(long id);
        Task<Result<long>> Create(BrandImageRequestDto dto);
        Task<Result<BrandImageResponseDto>> Update(BrandImageRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
