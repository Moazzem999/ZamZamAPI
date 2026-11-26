using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("Roles")]
    public class RoleEntity : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Description { get; set; }

        // Navigation Property
        public ICollection<UserRoleMappingEntity> UserRoleMappings { get; set; } = new List<UserRoleMappingEntity>();
    }
}
