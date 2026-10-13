using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.BrandImage
{
    public class BrandImageResponseDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
    }
}
