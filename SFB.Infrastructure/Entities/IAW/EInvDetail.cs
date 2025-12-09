using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_InvDetail")]
    public class EInvDetail
    {
        [Key,Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int TxnId { get; set; }

        [ForeignKey(nameof(TxnId))]
        public virtual EInventoryTxn? InventoryTxn { get; set; }

        [Required]
        public int NroProduct { get; set; }

        [ForeignKey(nameof(NroProduct))]
        public virtual EProduct? Product { get; set; }

        [Required]
        [MaxLength(3)]
        public string PresentCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyPresent { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }

    }
}
