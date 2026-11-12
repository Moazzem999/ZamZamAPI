using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.ContactInquiry
{
    public class ContactInquirySearchDto : PaginationDto
    {
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

