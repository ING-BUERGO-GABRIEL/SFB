using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.TRM.Shared.Models
{
    public class MTreasuryTxn
    {
        [Key, Required]
        public int TxnId { get; set; }

        // Referencia a la transacción origen (SalesTxnId, PurchaseTxnId, etc.)
        public int? TxnOrigin { get; set; }

        // Módulo origen: "SCM" (ventas), "PCM" (compras), "FTM" (tesorería), etc.
        [Required, MaxLength(3)]
        public string ModOrigin { get; set; } = null!;

        [Required, MaxLength(3)]
        public string Type { get; set; } = null!; // "ING", "EGR", "TRA" (según tu catálogo)

        [Required, MaxLength(3)]
        public string StatusCode { get; set; } = "PEN"; // PEN/APR/ANU

        [Required]
        public DateTime TxnDate { get; set; } = DateTime.UtcNow;

        // Caja origen (puede ser null según tu regla)
        public int? CashBoxOriginId { get; set; }
        [ForeignKey(nameof(CashBoxOriginId))]
        public virtual MCashBox? CashBoxOrigin { get; set; }

        // Caja destino (puede ser null según tu regla)
        public int? CashBoxDestId { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; } = "BOB";

        // Total = sumatoria del detalle
        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal GrandTotal { get; set; }

        public int? CashierId { get; set; }

        [MaxLength(250)]
        public string? Reference { get; set; }

        [Required]
        public bool Delete { get; set; } = false;

        public virtual ICollection<MTreasuryDetail> Details { get; set; } = new List<MTreasuryDetail>();
    }
}
