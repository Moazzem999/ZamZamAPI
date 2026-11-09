using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("Services")]
    public class ServiceEntity : BaseEntity
    {
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImagePath { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        
        // Navigation Property
        public ICollection<ServiceHighlightEntity> ServiceHighlights { get; set; } = new List<ServiceHighlightEntity>();
    }
}
