
using SFB.Infrastructure.Entities.IAW;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SFB.Infrastructure.Entities.PCM
{
    [Table("PCM_PurchaseDetail")]
    public class EPurchaseDetail
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int PurchaseId { get; set; }
        [ForeignKey(nameof(PurchaseId))]
        public virtual EPurchaseTxn? PurchaseTxn { get; set; }

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
        public decimal UnitCost { get; set; }

        [Required, Column(TypeName = "decimal(18,6)")]
        public decimal TotalCost { get; set; }
    }
}
