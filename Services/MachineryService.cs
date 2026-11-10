using ZamZamAPI.Dtos.Machinery;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class MachineryService(IMachineryRepository machineryRepository) : IMachineryService
    {
        private readonly IMachineryRepository machineryRepository = machineryRepository;

        public async Task<Result<PagedResult<MachineryResponseDto>>> GetAll(MachinerySearchDto searchDto)
        {
            return await machineryRepository.GetAll(searchDto);
        }

        public async Task<Result<List<MachineryResponseDto>>> GetAllForDisplay(long categoryId)
        {
            return await machineryRepository.GetAllForDisplay(categoryId);
        }

        public async Task<Result<MachineryResponseDto>> GetById(long id)
        {
            return await machineryRepository.GetById(id);
        }

        public async Task<Result<long>> Create(MachineryRequestDto dto)
        {
            return await machineryRepository.Create(dto);
        }

        public async Task<Result<MachineryResponseDto>> Update(MachineryRequestDto dto)
        {
            return await machineryRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await machineryRepository.Delete(id);
        }
    }
}

