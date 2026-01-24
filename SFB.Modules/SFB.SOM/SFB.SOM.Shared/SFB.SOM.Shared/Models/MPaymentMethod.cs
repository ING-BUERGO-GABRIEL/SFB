
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.SOM.Shared.Models
{
    public class MPaymentMethod
    {
        [Key, Required]
        public int PaymentId { get; set; }

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
