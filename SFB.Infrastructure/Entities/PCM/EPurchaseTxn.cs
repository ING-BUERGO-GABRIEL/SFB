using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.PCM
{
    [Table("PCM_PurchaseTxn")]
    public class EPurchaseTxn : Auditable
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TxnId { get; set; }
    }
}
