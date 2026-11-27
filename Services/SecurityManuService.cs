using ZamZamAPI.Dtos.SecurityManu;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class SecurityManuService(ISecurityManuRepository securityManuRepository) : ISecurityManuService
    {
        private readonly ISecurityManuRepository securityManuRepository = securityManuRepository;

        public async Task<Result<PagedResult<SecurityManuResponseDto>>> GetAll(SecurityManuSearchDto searchDto)
        {
            return await securityManuRepository.GetAll(searchDto);
        }

        public async Task<Result<List<SecurityManuResponseDto>>> GetAllParentMenu()
        {
            return await securityManuRepository.GetAllParentMenu();
        }

        public async Task<Result<SecurityManuResponseDto>> GetById(long id)
        {
            return await securityManuRepository.GetById(id);
        }

        public async Task<Result<long>> Create(SecurityManuRequestDto dto)
        {
            return await securityManuRepository.Create(dto);
        }

        public async Task<Result<SecurityManuResponseDto>> Update(SecurityManuRequestDto dto)
        {
            return await securityManuRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await securityManuRepository.Delete(id);
        }
    }
}
