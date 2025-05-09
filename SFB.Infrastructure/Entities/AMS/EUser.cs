using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGD.Infrastructure.Entities.AMS
{
    [Table("AMS_User")]
    public class EUser
    {
        [Key]
        [MaxLength(50)]
        [Required]
        public string NameUser { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Password { get; set; }

        [MaxLength(500)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [Required]
        [MaxLength(10)]
        public string State { get; set; }

        [Required]
        [MaxLength(2)]
        public string UserType { get; set; }

        public int? NroPerson { get; set; }

        //Virtuales
        //Navigation Properties
        [NotMapped]
        public virtual ICollection<EUserGroup> UserGroups { get; set; } = new List<EUserGroup>();

        [NotMapped]
        [ForeignKey("NroPerson")]
        public virtual EPerson? Person { get; set; }
    }
}
