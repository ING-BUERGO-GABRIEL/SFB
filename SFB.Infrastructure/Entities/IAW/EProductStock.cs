using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_ProductStock")]
    public class EProductStock
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroStock { get; set; }

        [Required]
        public int NroProduct { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Stock { get; set; }

        [NotMapped]
        public virtual EProduct? Product { get; set; }
    }
}
