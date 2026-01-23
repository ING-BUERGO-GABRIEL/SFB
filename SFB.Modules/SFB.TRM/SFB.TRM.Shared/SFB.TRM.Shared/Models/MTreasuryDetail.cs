using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.TRM.Shared.Models
{
    public class MTreasuryDetail
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int TxnId { get; set; }

        [Required, MaxLength(4)]
        public string PaymentMethodCode { get; set; } = null!; // "CASH","CRD","QR","TRF"

        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }

        [MaxLength(80)]
        public string? PaymentRef { get; set; }
    }
}
