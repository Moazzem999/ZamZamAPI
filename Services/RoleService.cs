using ZamZamAPI.Dtos.Role;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class RoleService(IRoleRepository roleRepository) : IRoleService
    {
        private readonly IRoleRepository roleRepository = roleRepository;

        public async Task<Result<PagedResult<RoleResponseDto>>> GetAll(RoleSearchDto searchDto)
        {
            return await roleRepository.GetAll(searchDto);
        }

        public async Task<Result<RoleResponseDto>> GetById(long id)
        {
            return await roleRepository.GetById(id);
        }

        public async Task<Result<long>> Create(RoleRequestDto dto)
        {
            return await roleRepository.Create(dto);
        }

        public async Task<Result<RoleResponseDto>> Update(RoleRequestDto dto)
        {
            return await roleRepository.Update(dto);
        }

        public async Task<Result<bool>> Delete(long id)
        {
            return await roleRepository.Delete(id);
        }
    }
}
