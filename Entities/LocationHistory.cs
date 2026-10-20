using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("LocationHistories")]
    public class LocationHistory : BaseEntity
    {
        public long EmployeeId { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? Accuracy { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? Speed { get; set; }
        public int? BatteryPercentage { get; set; }
        public DateTime RecordedAt { get; set; }


        [ForeignKey(nameof(EmployeeId))]
        public EmployeeEntity? Employee { get; set; }
    }
}
