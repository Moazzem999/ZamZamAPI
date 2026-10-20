using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.LocationHistory
{
    public class LocationHistorySearchDto : PaginationDto
    {
        public long? EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
