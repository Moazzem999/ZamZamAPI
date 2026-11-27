using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Role
{
    public class RoleResponseDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
