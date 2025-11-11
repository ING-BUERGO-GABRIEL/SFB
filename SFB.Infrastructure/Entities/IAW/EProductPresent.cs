using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_ProductPresent")]
    public class EProductPresent : Auditable
    {
        [Key]
        [Required,MaxLength(3)]
        public string PresentCode { get; set; }

        [ForeignKey(nameof(PresentCode))]
        public virtual EPresentation? Presentation { get; set; }

        [Key]
        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual EProduct? Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }

        public string? SerialNumber { get; set; }
    }
}
