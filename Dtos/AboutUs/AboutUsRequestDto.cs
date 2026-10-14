using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.AboutUs
{
    public class AboutUsRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Section label is required.")]
        [MaxLength(50, ErrorMessage = "Section label cannot exceed 50 characters.")]
        public string? SectionLabel { get; set; }

        [Required(ErrorMessage = "Main heading is required.")]
        [MaxLength(100, ErrorMessage = "Main heading cannot exceed 100 characters.")]
        public string MainHeading { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Introduction cannot exceed 500 characters.")]
        public string? Introduction { get; set; }

        [MaxLength(500, ErrorMessage = "Experience description cannot exceed 500 characters.")]
        public string? ExperienceDescription { get; set; }

        [MaxLength(500, ErrorMessage = "Production description cannot exceed 500 characters.")]
        public string? ProductionDescription { get; set; }

        [MaxLength(50, ErrorMessage = "Capability title cannot exceed 50 characters.")]
        public string? CapabilityTitle { get; set; }

        [MaxLength(250, ErrorMessage = "Capability description cannot exceed 250 characters.")]
        public string? CapabilityDescription { get; set; }
    }
}
