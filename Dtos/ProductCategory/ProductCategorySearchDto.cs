using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.ProductCategory
{
    public class ProductCategorySearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
    }
}

