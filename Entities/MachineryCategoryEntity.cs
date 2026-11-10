using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZamZamAPI.Entities.Common;

namespace ZamZamAPI.Entities
{
    [Table("MachineryCategories")]
    public class MachineryCategoryEntity : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;


        // Navigation Property
        public ICollection<MachineryEntity> Machineries { get; set; } = new List<MachineryEntity>();
    }
}
