using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.Machinery
{
    public class MachineryRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Machinery category is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Please select a valid machinery category.")]
        public long MachineryCategoryId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Card badge cannot exceed 50 characters.")]
        public string CardBadge { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public int DisplayOrder { get; set; } = 0;
    }
}

