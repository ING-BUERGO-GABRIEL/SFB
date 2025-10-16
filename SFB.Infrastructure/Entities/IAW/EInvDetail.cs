using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_InvDetail")]
    public class EInvDetail
    {
        [Key,Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int TxnId { get; set; }

        [Required]
        public int NroProduct { get; set; }

        [NotMapped]
        public virtual EProduct? Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }


    }
}
