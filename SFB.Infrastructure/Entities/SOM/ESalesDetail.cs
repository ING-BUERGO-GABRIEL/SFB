using SFB.Infrastructure.Entities.IAW;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.SOM
{
    [Table("SOM_SalesDetail")]
    public class ESalesDetail
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int SalesId { get; set; }
        [ForeignKey(nameof(SalesId))]
        public virtual ESalesTxn? SalesTxn { get; set; }

        [Required]
        public int NroProduct { get; set; }
        [ForeignKey(nameof(NroProduct))]
        public virtual EProduct? Product { get; set; }

        [Required, MaxLength(3)]
        public string PresentCode { get; set; } = null!;

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal QtyPresent { get; set; }

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }

        [Required, Column(TypeName = "decimal(18,6)")]
        public decimal UnitPrice { get; set; }

        [Required, Column(TypeName = "decimal(18,6)")]
        public decimal TotalPrice { get; set; }
    }
}
