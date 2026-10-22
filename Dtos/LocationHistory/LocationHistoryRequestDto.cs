namespace ZamZamAPI.Dtos.LocationHistory
{
    public class LocationHistoryRequestDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Accuracy { get; set; }
        public decimal? Speed { get; set; }
        public int? BatteryPercentage { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.Now;
    }
}
