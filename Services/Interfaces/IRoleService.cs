using ZamZamAPI.Dtos.Role;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Result<PagedResult<RoleResponseDto>>> GetAll(RoleSearchDto searchDto);
        Task<Result<List<RoleResponseDto>>> GetAllRoles();
        Task<Result<RoleResponseDto>> GetById(long id);
        Task<Result<long>> Create(RoleRequestDto dto);
        Task<Result<RoleResponseDto>> Update(RoleRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
