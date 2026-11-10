using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Machinery
{
    public class MachineryResponseDto : BaseDto
    {
        public long MachineryCategoryId { get; set; }
        public string MachineryCategoryName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CardBadge { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public int DisplayOrder { get; set; }
    }
}

