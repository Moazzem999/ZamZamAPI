using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.LocationHistory
{
    public class LocationHistoryFilterDto : PaginationDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
