using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.BrandImage
{
    public class BrandImageRequestDto
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Remarks cannot exceed 255 characters.")]
        public string Remarks { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
