using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.LocationHistory
{
    public class LocationHistoryResponseDto : BaseDto
    {
        public long EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Accuracy { get; set; }
        public decimal? Speed { get; set; }
        public int? BatteryPercentage { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
