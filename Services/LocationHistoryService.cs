using ZamZamAPI.Dtos.LocationHistory;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class LocationHistoryService(ILocationHistoryRepository locationHistoryRepository) : ILocationHistoryService
    {
        private readonly ILocationHistoryRepository locationHistoryRepository = locationHistoryRepository;

        public async Task<Result<PagedResult<LocationHistoryResponseDto>>> GetAll(LocationHistorySearchDto searchDto)
        {
            return await locationHistoryRepository.GetAll(searchDto);
        }

        public async Task<Result<LocationHistoryResponseDto>> GetById(long id)
        {
            return await locationHistoryRepository.GetById(id);
        }

        public async Task<Result<long>> Create(LocationHistoryRequestDto dto)
        {
            return await locationHistoryRepository.Create(dto);
        }
    }
}
