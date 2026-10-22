using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;
using ZamZamAPI.Shared.Enum;

namespace ZamZamAPI.Entities
{
    [Table("QuotationRequests")]
    public class QuotationRequestEntity : BaseEntity
    {
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(50)]
        public string CompanyName { get; set; } = string.Empty;
        public ServiceCategory ServiceCategory { get; set; }

        [MaxLength(50)]
        public string EstimatedQuantity { get; set; } = string.Empty;

        [MaxLength(250)]
        public string ProductReference { get; set; } = string.Empty;

        [MaxLength(500)]
        public string DetailedSpecifications { get; set; } = string.Empty;
    }
}
