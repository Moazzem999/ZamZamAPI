using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ZamZamAPI.Dtos.Product
{
    public class ProductRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Product category is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Please select a valid product category.")]
        public long ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Card badge cannot exceed 50 characters.")]
        public string CardBadge { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Technical specs cannot exceed 250 characters.")]
        public string TechnicalSpecs { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }
}

