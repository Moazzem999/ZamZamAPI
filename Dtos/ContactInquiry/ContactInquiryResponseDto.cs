using ZamZamAPI.Dtos.Common;

namespace ZamZamAPI.Dtos.ContactInquiry
{
    public class ContactInquiryResponseDto : BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

