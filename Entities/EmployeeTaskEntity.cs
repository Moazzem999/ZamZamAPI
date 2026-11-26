using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("EmployeeTasks")]
    public class EmployeeTaskEntity : BaseEntity
    {
        public long EmployeeId { get; set; }

        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime ScheduledOn { get; set; }

        // Navigation Property
        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity Employee { get; set; } = null!;
    }
}
