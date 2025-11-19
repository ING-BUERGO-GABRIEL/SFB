using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.IAW.Shared.Models
{
    public class MProductPresent
    {
        [Key]
        [Required, MaxLength(3)]
        public string PresentCode { get; set; }

        [Key]
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }

        public string? SerialNumber { get; set; }

    }
}
