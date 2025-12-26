using SFB.IAW.Shared.Models;
using SFB.PCM.Shared.Sealed;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.PCM.Shared.Models
{
    public class MPurchaseTxn
    {
        [Key, Required]
        public int TxnId { get; set; }

        [Required]
        public int SupplierId { get; set; }
        public virtual MSupplier? Supplier { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; } = "BOB";

        [Required]
        public int WarehouseId { get; set; }
        public virtual MWarehouse? Warehouse { get; set; }

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal GrandTotal { get; set; }

        [Required, MaxLength(3)]
        public string StatusCode { get; set; } = PurchaseStatus.Activo.Code;
        public PurchaseStatus Status => PurchaseStatus.FromCode(StatusCode);

        [Required, MaxLength(3)]
        public string Type { get; set; }
        public string NameType => PurchaseType.FromCode(Type).Name;

        [Required]
        public bool Delete { get; set; } = false;

        [MaxLength(250)]
        public string? Reference { get; set; }

        public virtual ICollection<MPurchaseDetail> Details { get; set; } = new List<MPurchaseDetail>();
    }
}
