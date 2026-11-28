using ZamZamAPI.Dtos.SecurityRolePermission;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface ISecurityRolePermissionRepository
    {
        Task<Result<List<SecurityRolePermissionResponseDto>>> GetAll(SecurityRolePermissionSearchDto searchDto);
        Task<Result<List<SecurityRolePermissionResponseDto>>> GetAllByRoleId(long roleId);
        Task<Result<List<long>>> Create(List<SecurityRolePermissionRequestDto> dtos);
        Task<Result<List<SecurityRolePermissionResponseDto>>> Update(List<SecurityRolePermissionRequestDto> dtos);
    }
}
