using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.ProductCategory
{
    public class ProductCategoryResponseDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}

