using ZamZamAPI.Dtos.Common;
using ZamZamAPI.Shared.Enum;

namespace ZamZamAPI.Dtos.QuotationRequest
{
    public class QuotationRequestSearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
        public ServiceCategory? ServiceCategory { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
