using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("UserRoleMappings")]
    public class UserRoleMappingEntity : BaseEntity
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public virtual RoleEntity Role { get; set; } = null!;
    }
}
