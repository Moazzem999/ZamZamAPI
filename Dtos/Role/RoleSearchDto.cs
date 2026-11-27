using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Role
{
    public class RoleSearchDto : PaginationDto
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
