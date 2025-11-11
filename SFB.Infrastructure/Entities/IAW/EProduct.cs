using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_Product")]
    public class EProduct : Auditable
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroProduct { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public string? SerialNumber { get; set; }

        [Required]
        public bool IsPurchases { get; set; } = true;

        [Required]
        public bool IsSales { get; set; } = true;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price {  get; set; }

        [Required,MaxLength(3)]
        public string PresentCode { get; set; }

        [ForeignKey(nameof(PresentCode))]
        public virtual EPresentation? Presentation { get; set; }

        [Required]
        public bool Status { get; set; } = true;

        public virtual ICollection<EProductPresent>? ProductPresent { get; set; } = new List<EProductPresent>();

        public virtual ICollection<EInvDetail>? InvDetails { get; set; }
    }
}
