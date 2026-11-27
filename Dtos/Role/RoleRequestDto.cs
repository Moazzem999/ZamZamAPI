namespace ZamZamAPI.Dtos.Role
{
    public class RoleRequestDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
