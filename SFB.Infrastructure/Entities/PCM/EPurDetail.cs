
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.PCM
{
    [Table("PCM_PurDetail")]
    public class EPurDetail
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }

        [Required]
        public int TxnId { get; set; }

    }
}
