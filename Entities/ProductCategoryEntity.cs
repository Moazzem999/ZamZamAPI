using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("ProductCategories")]
    public class ProductCategoryEntity : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;


        // Navigation Property
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
