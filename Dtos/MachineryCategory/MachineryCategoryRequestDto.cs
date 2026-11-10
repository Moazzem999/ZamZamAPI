using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.MachineryCategory
{
    public class MachineryCategoryRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;
    }
}

