using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("BrandImages")]
    public class BrandImageEntity : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Remarks { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ImagePath { get; set; }
    }
}
