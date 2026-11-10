using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Machinery
{
    public class MachinerySearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
    }
}

