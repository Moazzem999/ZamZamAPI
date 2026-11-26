using ZamZamAPI.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZamZamAPI.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public long? EmployeeId { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(EmployeeId))]
        public virtual EmployeeEntity? Employee { get; set; }
        public ICollection<UserRoleMappingEntity> UserRoleMappings { get; set; } = new List<UserRoleMappingEntity>();
    }
}
