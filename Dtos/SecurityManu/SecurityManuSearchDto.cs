using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.SecurityManu
{
    public class SecurityManuSearchDto : PaginationDto
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
