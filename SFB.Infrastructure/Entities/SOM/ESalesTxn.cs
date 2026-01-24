using SFB.Infrastructure.Entities.AMS;
using SFB.Infrastructure.Entities.CMS;
using SFB.Infrastructure.Entities.IAW;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.SOM
{
    [Table("SOM_SalesTxn")]
    public class ESalesTxn : Auditable
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TxnId { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual ECustomer? Customer { get; set; }

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; } = "BOB";

        [Required]
        public int WarehouseId { get; set; }
        [ForeignKey(nameof(WarehouseId))]
        public virtual EWarehouse Warehouse { get; set; } = null!;

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal GrandTotal { get; set; }

        [Required, MaxLength(3)]
        public string StatusCode { get; set; } = "ACT";

        [Required, MaxLength(3)]
        public string Type { get; set; } = null!;

        [Required]
        public bool Delete { get; set; } = false;

        [MaxLength(250)]
        public string? Reference { get; set; }

        public virtual ICollection<ESalesDetail> Details { get; set; } = new List<ESalesDetail>();

        public virtual ICollection<EPaymentMethod> PaymentMethods { get; set; } = new List<EPaymentMethod>();
    }
}
