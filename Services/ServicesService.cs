using ZamZamAPI.Dtos.Service;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class ServicesService(IServicesRepository servicesRepository) : IServicesService
    {
        private readonly IServicesRepository servicesRepository = servicesRepository;

        public async Task<Result<PagedResult<ServiceResponseDto>>> GetAll(ServiceSearchDto searchDto)
        {
            return await servicesRepository.GetAll(searchDto);
        }

        public async Task<Result<List<ServiceResponseDto>>> GetAllForDisplay()
        {
            return await servicesRepository.GetAllForDisplay();
        }

        public async Task<Result<ServiceResponseDto>> GetById(long id)
        {
            return await servicesRepository.GetById(id);
        }

        public async Task<Result<long>> Create(ServiceRequestDto dto)
        {
            return await servicesRepository.Create(dto);
        }

        public async Task<Result<ServiceResponseDto>> Update(ServiceRequestDto dto)
        {
            return await servicesRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await servicesRepository.Delete(id);
        }
    }
}

