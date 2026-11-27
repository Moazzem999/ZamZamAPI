using ZamZamAPI.Dtos.UserRoleMapping;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class UserRoleMappingService(IUserRoleMappingRepository userRoleMappingRepository) : IUserRoleMappingService
    {
        private readonly IUserRoleMappingRepository userRoleMappingRepository = userRoleMappingRepository;

        public async Task<Result<PagedResult<UserRoleMappingResponseDto>>> GetAll(UserRoleMappingSearchDto searchDto)
        {
            return await userRoleMappingRepository.GetAll(searchDto);
        }

        public async Task<Result<UserRoleMappingResponseDto>> GetById(long id)
        {
            return await userRoleMappingRepository.GetById(id);
        }

        public async Task<Result<long>> Create(UserRoleMappingRequestDto dto)
        {
            return await userRoleMappingRepository.Create(dto);
        }

        public async Task<Result<UserRoleMappingResponseDto>> Update(UserRoleMappingRequestDto dto)
        {
            return await userRoleMappingRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await userRoleMappingRepository.Delete(id);
        }
    }
}
