using ZamZamAPI.Dtos.UserRoleMapping;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IUserRoleMappingRepository
    {
        Task<Result<PagedResult<UserRoleMappingResponseDto>>> GetAll(UserRoleMappingSearchDto searchDto);
        Task<Result<UserRoleMappingResponseDto>> GetById(long id);
        Task<Result<long>> Create(UserRoleMappingRequestDto dto);
        Task<Result<UserRoleMappingResponseDto>> Update(UserRoleMappingRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
