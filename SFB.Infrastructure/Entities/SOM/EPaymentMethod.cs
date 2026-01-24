using SFB.Infrastructure.Entities.TRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.SOM
{
    [Table("SOM_PaymentMethod")]
    public class EPaymentMethod
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        public int TxnId { get; set; }
        [ForeignKey(nameof(TxnId))]
        public virtual ESalesTxn? SalesTxn { get; set; }

        [Required, MaxLength(4)]
        public string PaymentMethodCode { get; set; } = null!; // "CASH","CRD","QR","TRF"

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [MaxLength(80)]
        public string? PaymentRef { get; set; } // nro transferencia, voucher, QR ref, etc.
    }
}
