using ZamZamAPI.Dtos.Common;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services.Interfaces
{
    public interface IDropdownService
    {
        Task<Result<List<DropdownDto>>> GetAllMaritalStatus();
        Task<Result<List<DropdownDto>>> GetAllReligion();
        Task<Result<List<DropdownDto>>> GetAllServiceCategory();
    }
}
