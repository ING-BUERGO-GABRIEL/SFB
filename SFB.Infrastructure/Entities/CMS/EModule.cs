using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGD.Infrastructure.Entities.CMS
{
    [Table("CMS_Module")]
    public class EModule
    {
        [Key]
        [MaxLength(3)]
        [Required]
        public string CodModule { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? Icon { get; set; }

        //Vuartual
        [NotMapped]
        public virtual ICollection<EGroup> Groups { get; set; } = new List<EGroup>();
    }
}
