namespace ZamZamAPI.Dtos.SecurityManu
{
    public class SecurityManuRequestDto
    {
        public long Id { get; set; }
        public long? ParentManuId { get; set; }
        public string ManuName { get; set; } = string.Empty;
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? Icon { get; set; }
        public int? Sort { get; set; }
    }
}
