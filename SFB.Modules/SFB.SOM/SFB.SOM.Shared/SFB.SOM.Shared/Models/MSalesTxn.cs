using SFB.AMS.Shared.Models;
using SFB.IAW.Shared.Models;
using SFB.SOM.Shared.Sealed;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.SOM.Shared.Models
{
    public class MSalesTxn
    {
        [Key, Required]
        public int TxnId { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public virtual MCustomer? Customer { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; } = "BOB";

        [Required]
        public int WarehouseId { get; set; }
        public virtual MWarehouse? Warehouse { get; set; }

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal GrandTotal { get; set; }

        [Required, MaxLength(3)]
        public string StatusCode { get; set; } = SalesStatus.Activo.Code;
        public SalesStatus Status => SalesStatus.FromCode(StatusCode);

        [Required, MaxLength(3)]
        public string Type { get; set; } = SalesType.Venta.Code;
        public string NameType => SalesType.FromCode(Type).Name;

        [Required]
        public bool Delete { get; set; } = false;

        [MaxLength(250)]
        public string? Reference { get; set; }

        public virtual ICollection<MSalesDetail> Details { get; set; } = new List<MSalesDetail>();
    }
}
