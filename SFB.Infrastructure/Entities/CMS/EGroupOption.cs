using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Infrastructure.Entities.CMS
{
    [Table("CMS_GroupOption")]
    public class EGroupOption
    {
        [Key]
        [Required]
        public int NroGroup { get; set; }

        [Key]
        [Required]
        [MaxLength(20)]
        public string CodOption { get; set; }

        //Virtuales
        [NotMapped]
        [ForeignKey("NroGroup")]
        public virtual EGroup Group { get; set; }

        [NotMapped]
        [ForeignKey("CodOption")]
        public virtual EOptionMenu OptionMenu { get; set; }
    }
}
