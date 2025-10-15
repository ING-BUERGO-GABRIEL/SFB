using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_InventoryTxn")]
    public class EInventoryTxn
    {
        [Key]
        [Required]
        public int TxnId { get; set; }

        public int? TxnOrigin { get; set; }

        [Required]
        [MaxLength(3)]
        public string ModOrigin { get; set; }
    }
}
