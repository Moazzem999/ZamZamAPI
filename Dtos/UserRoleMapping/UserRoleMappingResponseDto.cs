using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.UserRoleMapping
{
    public class UserRoleMappingResponseDto : BaseDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
