using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("ServiceHighlights")]
    public class ServiceHighlightEntity : BaseEntity
    {
        public long ServiceId { get; set; }

        [MaxLength(60)]
        public string HighlightText { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;


        // Navigation Property
        [ForeignKey(nameof(ServiceId))]
        public virtual ServiceEntity Service { get; set; } = null!;
    }
}
