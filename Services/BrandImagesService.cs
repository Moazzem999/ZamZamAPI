using ZamZamAPI.Dtos.BrandImage;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class BrandImagesService(IBrandImagesRepository brandImagesRepository) : IBrandImagesService
    {
        public async Task<Result<List<BrandImageResponseDto>>> GetAll()
        {
            return await brandImagesRepository.GetAll();
        }

        public async Task<Result<BrandImageResponseDto>> GetById(long id)
        {
            return await brandImagesRepository.GetById(id);
        }

        public async Task<Result<long>> Create(BrandImageRequestDto dto)
        {
            return await brandImagesRepository.Create(dto);
        }

        public async Task<Result<bool>> Update(BrandImageRequestDto dto)
        {
            return await brandImagesRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await brandImagesRepository.Delete(id);
        }
    }
}
