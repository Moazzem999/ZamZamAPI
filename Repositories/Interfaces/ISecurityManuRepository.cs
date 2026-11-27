using ZamZamAPI.Dtos.SecurityManu;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface ISecurityManuRepository
    {
        Task<Result<PagedResult<SecurityManuResponseDto>>> GetAll(SecurityManuSearchDto searchDto);
        Task<Result<List<SecurityManuResponseDto>>> GetAllParentMenu();
        Task<Result<SecurityManuResponseDto>> GetById(long id);
        Task<Result<long>> Create(SecurityManuRequestDto dto);
        Task<Result<SecurityManuResponseDto>> Update(SecurityManuRequestDto dto);
        Task<Result<bool>> Delete(long id);
    }
}
