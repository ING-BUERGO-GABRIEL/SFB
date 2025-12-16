using SFB.Infrastructure.Entities.CMS;
using SFB.Infrastructure.Entities.IAW;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.PCM
{
    [Table("PCM_PurchaseTxn")]
    public class EPurchaseTxn : Auditable
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TxnId { get; set; }

        [Required]
        public int SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public virtual ESupplier? Supplier { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; } = "BOB";

        [Required]
        public int WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public virtual EWarehouse Warehouse { get; set; }

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal GrandTotal { get; set; }

        [Required, MaxLength(3)]
        public string StatusCode { get; set; } = "ACT";

        [Required, MaxLength(3)]
        public string Type { get; set; }

        [Required]
        public bool Delete { get; set; } = false;

        [MaxLength(250)]
        public string? Reference { get; set; }

        public virtual ICollection<EPurchaseDetail> Details { get; set; } = new List<EPurchaseDetail>();

    }
}
