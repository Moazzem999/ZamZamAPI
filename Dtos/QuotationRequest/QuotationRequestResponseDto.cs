using ZamZamAPI.Dtos.Common;
using ZamZamAPI.Shared.Enum;

namespace ZamZamAPI.Dtos.QuotationRequest
{
    public class QuotationRequestResponseDto : BaseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public ServiceCategory ServiceCategory { get; set; }
        public string ServiceCategoryName { get; set; } = string.Empty;
        public string EstimatedQuantity { get; set; } = string.Empty;
        public string ProductReference { get; set; } = string.Empty;
        public string DetailedSpecifications { get; set; } = string.Empty;
    }
}
