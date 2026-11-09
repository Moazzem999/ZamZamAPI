using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.Service
{
    public class ServiceResponseDto : BaseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public List<ServiceHighlightResponseDto> Highlights { get; set; } = new();
    }
}

