using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.MachineryCategory
{
    public class MachineryCategorySearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
    }
}

