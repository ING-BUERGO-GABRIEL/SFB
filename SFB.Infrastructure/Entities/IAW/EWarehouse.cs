using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_Warehouse")]
    public class EWarehouse : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarehouseId { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string? Location { get; set; }

        [Required]
        public bool Status { get; set; } = true;
    }
}
