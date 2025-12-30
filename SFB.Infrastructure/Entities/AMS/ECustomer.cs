using SFB.Infrastructure.Entities.CMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.AMS
{
    [Table("AMS_Customer")]
    public class ECustomer : Auditable
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        public int NroPerson { get; set; }

        [Required,ForeignKey(nameof(NroPerson))]
        public EPerson Person { get; set; } = null!;

    }
}
