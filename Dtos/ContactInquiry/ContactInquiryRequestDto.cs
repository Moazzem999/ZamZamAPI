using System.ComponentModel.DataAnnotations;

namespace ZamZamAPI.Dtos.ContactInquiry
{
    public class ContactInquiryRequestDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Subject cannot exceed 250 characters.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; } = string.Empty;
    }
}

