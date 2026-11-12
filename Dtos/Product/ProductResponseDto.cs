using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Product
{
    public class ProductResponseDto : BaseDto
    {
        public long ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CardBadge { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TechnicalSpecs { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public int DisplayOrder { get; set; }
    }
}

