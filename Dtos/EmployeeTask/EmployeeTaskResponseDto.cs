using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.EmployeeTask
{
    public class EmployeeTaskResponseDto : BaseDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledOn { get; set; }
    }
}
