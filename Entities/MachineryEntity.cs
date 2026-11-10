using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("Machineries")]
    public class MachineryEntity : BaseEntity
    {
        public long MachineryCategoryId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CardBadge { get; set; } = string.Empty;       

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImagePath { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;


        // Navigation Property
        [ForeignKey(nameof(MachineryCategoryId))]
        public virtual MachineryCategoryEntity MachineryCategory { get; set; } = null!;
    }
}
