using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("AboutUs")]
    public class AboutUsEntity : BaseEntity
    {
        [MaxLength(50)]
        public string SectionLabel { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string MainHeading { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Introduction { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ExperienceDescription { get; set; } = string.Empty;

        [MaxLength(500)]
        public string ProductionDescription { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CapabilityTitle { get; set; } = string.Empty;

        [MaxLength(250)]
        public string CapabilityDescription { get; set; } = string.Empty;
    }
}
