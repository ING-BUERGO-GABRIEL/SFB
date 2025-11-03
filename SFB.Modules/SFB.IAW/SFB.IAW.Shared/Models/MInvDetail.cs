using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.IAW.Shared.Models
{
    public class MInvDetail
    {
        [Key, Required]
        public int DetailId { get; set; }

        [Required]
        public int TxnId { get; set; }


        [Required]
        public int NroProduct { get; set; }

        public virtual MProduct? Product { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }

        public virtual string? _ProdName => Product?.Name;
    }
}
