using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Service
{
    public class ServiceHighlightResponseDto : BaseDto
    {
        public long ServiceId { get; set; }
        public string HighlightText { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}

