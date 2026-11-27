using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("SecurityRolePermissions")]
    public class SecurityRolePermissionEntity : BaseEntity
    {
        public long RoleId { get; set; }
        public long SecurityManuId { get; set; }

        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsReport { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(RoleId))]
        public virtual RoleEntity Role { get; set; } = null!;

        [ForeignKey(nameof(SecurityManuId))]
        public virtual SecurityManuEntity SecurityManu { get; set; } = null!;
    }
}
