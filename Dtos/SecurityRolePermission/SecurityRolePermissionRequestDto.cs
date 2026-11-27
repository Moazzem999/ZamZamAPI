namespace ZamZamAPI.Dtos.SecurityRolePermission
{
    public class SecurityRolePermissionRequestDto
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long SecurityManuId { get; set; }
        public bool IsView { get; set; }
        public bool IsInsert { get; set; }
        public bool IsModify { get; set; }
        public bool IsDelete { get; set; }
        public bool IsReport { get; set; }
    }
}
