using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.Service
{
    public class ServiceHighlightRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Highlight text is required.")]
        [MaxLength(60, ErrorMessage = "Highlight text cannot exceed 60 characters.")]
        public string HighlightText { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}

