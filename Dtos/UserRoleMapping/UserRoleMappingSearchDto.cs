using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.UserRoleMapping
{
    public class UserRoleMappingSearchDto : PaginationDto
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
