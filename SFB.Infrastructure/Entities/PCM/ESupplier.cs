using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SFB.Infrastructure.Entities.PCM
{
    [Table("PCM_Supplier")]
    public class ESupplier: Auditable
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required, MaxLength(70)]
        public string Name { get; set; }

        [Required,MaxLength(300)]
        public string Address { get; set; }
    }
}
