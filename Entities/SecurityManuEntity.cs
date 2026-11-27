using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("SecurityManus")]
    public class SecurityManuEntity : BaseEntity
    {
        public long? ParentManuId { get; set; }

        [MaxLength(100)]
        public string ManuName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? ControllerName { get; set; }

        [MaxLength(50)]
        public string? ActionName { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        public int? Sort { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(ParentManuId))]
        public virtual SecurityManuEntity? ParentManu { get; set; }

        public ICollection<SecurityManuEntity> ChildManus { get; set; } = new List<SecurityManuEntity>();
        public ICollection<SecurityRolePermissionEntity> RolePermissions { get; set; } = new List<SecurityRolePermissionEntity>();
    }
}
