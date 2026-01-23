using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.TRM
{
    [Table("TRM_TreasuryDetail")]
    public class ETreasuryDetail
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int TxnId { get; set; }
        [ForeignKey(nameof(TxnId))]
        public virtual ETreasuryTxn? TreasuryTxn { get; set; }

        [Required, MaxLength(3)]
        public string PaymentMethodCode { get; set; } = null!; // "EFE","TAR","QR ","TRF"

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [MaxLength(80)]
        public string? PaymentRef { get; set; } // nro transferencia, voucher, QR ref, etc.
    }
}
