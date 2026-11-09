using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.Service
{
    public class ServiceRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        public IFormFile? Image { get; set; }

        public List<ServiceHighlightRequestDto>? Highlights { get; set; } = new();

        public string? HighlightsJson { get; set; }
    }
}

