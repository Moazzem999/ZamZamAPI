using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Service
{
    public class ServiceSearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
    }
}

