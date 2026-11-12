using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.ProductCategory
{
    public class ProductCategoryRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}

