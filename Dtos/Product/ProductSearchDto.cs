using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Product
{
    public class ProductSearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
    }
}

