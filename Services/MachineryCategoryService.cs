using ZamZamAPI.Dtos.MachineryCategory;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class MachineryCategoryService(IMachineryCategoryRepository machineryCategoryRepository) : IMachineryCategoryService
    {
        private readonly IMachineryCategoryRepository machineryCategoryRepository = machineryCategoryRepository;

        public async Task<Result<PagedResult<MachineryCategoryResponseDto>>> GetAll(MachineryCategorySearchDto searchDto)
        {
            return await machineryCategoryRepository.GetAll(searchDto);
        }

        public async Task<Result<List<MachineryCategoryResponseDto>>> GetAllForDisplay()
        {
            return await machineryCategoryRepository.GetAllForDisplay();
        }

        public async Task<Result<MachineryCategoryResponseDto>> GetById(long id)
        {
            return await machineryCategoryRepository.GetById(id);
        }

        public async Task<Result<long>> Create(MachineryCategoryRequestDto dto)
        {
            return await machineryCategoryRepository.Create(dto);
        }

        public async Task<Result<MachineryCategoryResponseDto>> Update(MachineryCategoryRequestDto dto)
        {
            return await machineryCategoryRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await machineryCategoryRepository.Delete(id);
        }
    }
}

