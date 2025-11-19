using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SFB.IAW.Shared.Models
{
    public class MProduct
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroProduct { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public string? SerialNumber { get; set; }

        [Required]
        public bool IsPurchases { get; set; } = true;

        [Required]
        public bool IsSales { get; set; } = true;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required, MaxLength(3)]
        public string PresentCode { get; set; }

        public ICollection<MProductPresent>? ProductPresent { get; set; } = new List<MProductPresent>();

    }

}
