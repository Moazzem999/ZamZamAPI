using ZamZamAPI.Dtos.Common;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Repositories.Interfaces
{
    public interface IDropdownRepository
    {
        Task<Result<List<DropdownDto>>> GetAllMaritalStatus();
        Task<Result<List<DropdownDto>>> GetAllReligion();
    }
}
