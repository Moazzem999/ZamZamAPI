using ZamZamAPI.Dtos.SecurityRolePermission;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface ISecurityRolePermissionService
    {
        Task<Result<List<SecurityRolePermissionResponseDto>>> GetAll(SecurityRolePermissionSearchDto searchDto);
        Task<Result<List<long>>> Create(List<SecurityRolePermissionRequestDto> dtos);
        Task<Result<List<SecurityRolePermissionResponseDto>>> Update(List<SecurityRolePermissionRequestDto> dtos);
    }
}
