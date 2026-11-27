using ZamZamAPI.Dtos.SecurityRolePermission;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class SecurityRolePermissionService(ISecurityRolePermissionRepository securityRolePermissionRepository) : ISecurityRolePermissionService
    {
        private readonly ISecurityRolePermissionRepository securityRolePermissionRepository = securityRolePermissionRepository;

        public async Task<Result<List<SecurityRolePermissionResponseDto>>> GetAll(SecurityRolePermissionSearchDto searchDto)
        {
            return await securityRolePermissionRepository.GetAll(searchDto);
        }

        public async Task<Result<List<long>>> Create(List<SecurityRolePermissionRequestDto> dtos)
        {
            return await securityRolePermissionRepository.Create(dtos);
        }

        public async Task<Result<List<SecurityRolePermissionResponseDto>>> Update(List<SecurityRolePermissionRequestDto> dtos)
        {
            return await securityRolePermissionRepository.Update(dtos);
        }
    }
}
