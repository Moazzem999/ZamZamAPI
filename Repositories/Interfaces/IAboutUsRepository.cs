using ZamZamAPI.Dtos.AboutUs;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IAboutUsRepository
    {
        Task<Result<List<AboutUsResponseDto>>> GetAll();
        Task<Result<AboutUsResponseDto>> GetById(long id);
        Task<Result<AboutUsResponseDto>> GetActiveAboutUs();
        Task<Result<long>> Create(AboutUsRequestDto dto);
        Task<Result<AboutUsResponseDto>> Update(AboutUsRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
