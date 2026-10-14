using ZamZamAPI.Dtos.AboutUs;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class AboutUsService(IAboutUsRepository aboutUsRepository) : IAboutUsService
    {
        public async Task<Result<List<AboutUsResponseDto>>> GetAll()
        {
            return await aboutUsRepository.GetAll();
        }

        public async Task<Result<AboutUsResponseDto>> GetById(long id)
        {
            return await aboutUsRepository.GetById(id);
        }

        public async Task<Result<AboutUsResponseDto>> GetActiveAboutUs()
        {
            return await aboutUsRepository.GetActiveAboutUs();
        }

        public async Task<Result<long>> Create(AboutUsRequestDto dto)
        {
            return await aboutUsRepository.Create(dto);
        }

        public async Task<Result<AboutUsResponseDto>> Update(AboutUsRequestDto dto)
        {
            return await aboutUsRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await aboutUsRepository.Delete(id);
        }
    }
}
