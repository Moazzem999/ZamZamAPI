using ZamZamAPI.Dtos.Common;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Services.Interfaces;
using ZamZamAPI.Shared.Infrastructure;

namespace ZamZamAPI.Services
{
    public class DropdownService(IDropdownRepository dropdownRepository) : IDropdownService
    {
        private readonly IDropdownRepository dropdownRepository = dropdownRepository;

        public async Task<Result<List<DropdownDto>>> GetAllMaritalStatus()
        {
            return await dropdownRepository.GetAllMaritalStatus();
        }

        public async Task<Result<List<DropdownDto>>> GetAllReligion()
        {
            return await dropdownRepository.GetAllReligion();
        }

        public async Task<Result<List<DropdownDto>>> GetAllServiceCategory()
        {
            return await dropdownRepository.GetAllServiceCategory();
        }
    }
}
