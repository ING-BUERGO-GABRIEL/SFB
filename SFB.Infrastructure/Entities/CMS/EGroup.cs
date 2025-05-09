using SGD.Infrastructure.Entities.AMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGD.Infrastructure.Entities.CMS
{
    [Table("CMS_Group")]
    public class EGroup
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroGroup { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(3)]
        public string CodModule { get; set; }

        //Virtuales
        //Forenign Key
        [NotMapped]
        [ForeignKey("CodModule")]
        public virtual EModule Module { get; set; }

        //Navigation
        [NotMapped]
        public virtual ICollection<EUserGroup> UserGroups { get; set; } = new List<EUserGroup>();
        [NotMapped]
        public virtual ICollection<EGroupOption> GroupOptions { get; set; } = new List<EGroupOption>();

    }
}
