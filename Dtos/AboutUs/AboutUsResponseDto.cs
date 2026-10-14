using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.AboutUs
{
    public class AboutUsResponseDto : BaseDto
    {
        public string SectionLabel { get; set; } = string.Empty;
        public string MainHeading { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;
        public string ExperienceDescription { get; set; } = string.Empty;
        public string ProductionDescription { get; set; } = string.Empty;
        public string CapabilityTitle { get; set; } = string.Empty;
        public string CapabilityDescription { get; set; } = string.Empty;
    }
}
