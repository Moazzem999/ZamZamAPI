using ZamZamAPI.Dtos.Common;
using ZamZamAPI.Repositories.Interfaces;
using ZamZamAPI.Shared.Enum;
using ZamZamAPI.Shared.Infrastructure;
using ZamZamAPI.Shared.Utilities;

namespace ZamZamAPI.Repositories
{
    public class DropdownRepository : IDropdownRepository
    {
        public async Task<Result<List<DropdownDto>>> GetAllMaritalStatus()
        {
            var data = Enum.GetValues<MaritalStatus>()
                .Select(x => new DropdownDto
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                })
                .ToList();

            return await Result<List<DropdownDto>>.SuccessAsync("", data);
        }

        public async Task<Result<List<DropdownDto>>> GetAllReligion()
        {
            var data = Enum.GetValues<Religion>()
                .Select(x => new DropdownDto
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                })
                .ToList();

            return await Result<List<DropdownDto>>.SuccessAsync("", data);
        }

        public async Task<Result<List<DropdownDto>>> GetAllServiceCategory()
        {
            var data = Enum.GetValues<ServiceCategory>()
                .Select(x => new DropdownDto
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                })
                .ToList();

            return await Result<List<DropdownDto>>.SuccessAsync("", data);
        }
    }
}
