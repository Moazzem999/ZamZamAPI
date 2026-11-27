using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.SecurityRolePermission
{
    public class SecurityRolePermissionResponseDto : BaseDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public long SecurityManuId { get; set; }
        public string SecurityManuName { get; set; } = string.Empty;
        public long? ParentManuId { get; set; }
        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsReport { get; set; }
    }
}
