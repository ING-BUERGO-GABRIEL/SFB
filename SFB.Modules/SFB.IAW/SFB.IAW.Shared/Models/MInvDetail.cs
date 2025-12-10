using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

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
        [MaxLength(3)]
        public string PresentCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyPresent { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyProduct { get; set; }

        public virtual string? _ProdName => Product?.Name;

        public virtual List<dynamic> PresentItems => new List<dynamic>{
            new { 
                Presentation = new{ Code = PresentCode, Name= PresentCode},
                QtyProduct  = QtyPresent > 0 && QtyProduct > 0 ? QtyProduct / QtyPresent : 1,
            }
        };
    }
}
