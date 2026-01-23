using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SFB.Infrastructure.Entities.TRM
{
    [Table("TRM_CashBox")]
    public class ECashBox : Auditable
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CashBoxId { get; set; }

        [Required, MaxLength(80)]
        public string Name { get; set; } = null!; // "Caja Juan", "Caja Chica Central"

        [Required, MaxLength(3)]
        public string Type { get; set; } = null!; // "POS" (cajero) / "PCH" (caja chica)

        [Required, MaxLength(3)]
        public string CurrencyCode { get; set; } = "BOB";

        // Saldo actual (opcional, pero muy útil para consultas rápidas)
        [Required, Column(TypeName = "decimal(18,4)")]
        public decimal CurrentBalance { get; set; }

        [Required]
        public bool Active { get; set; } = true;
    }
}
