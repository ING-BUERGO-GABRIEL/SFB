using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.AMS
{
    [Table("AMS_UserGroup")]
    public class EUserGroup
    {
        [Key]
        [Required]
        public int NroGroup { get; set; }

        [Key]
        [MaxLength(50)]
        [Required]
        public string NameUser { get; set; }

        //Virtuales
        [NotMapped]
        [ForeignKey("NroGroup")]
        public virtual EGroup Group { get; set; }

        [NotMapped]
        [ForeignKey("NameUser")]
        public virtual EUser User { get; set; }
    }
}
