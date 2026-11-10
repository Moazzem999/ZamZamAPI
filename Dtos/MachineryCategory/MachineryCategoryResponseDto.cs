using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.MachineryCategory
{
    public class MachineryCategoryResponseDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}

