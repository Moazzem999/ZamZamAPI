using ZamZamAPI.Dtos.Role;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Result<PagedResult<RoleResponseDto>>> GetAll(RoleSearchDto searchDto);
        Task<Result<RoleResponseDto>> GetById(long id);
        Task<Result<long>> Create(RoleRequestDto dto);
        Task<Result<RoleResponseDto>> Update(RoleRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
