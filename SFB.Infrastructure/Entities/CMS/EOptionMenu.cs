using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGD.Infrastructure.Entities.CMS
{
    [Table("CMS_OptionMenu")]
    public class EOptionMenu
    {
        [MaxLength(20)]
        [Required]
        public string CodOption { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [MaxLength(20)]
        public string? CodGruOption { get; set; }

        [MaxLength(100)]
        public string? Route { get; set; }

        //Realciones Virtuales
        //foerign key
        [NotMapped]
        [ForeignKey("CodGruOption")]
        public virtual EOptionMenu? OptionMenu { get; set; }

        //Navigation
        [NotMapped]
        public virtual ICollection<EOptionMenu> OptionMenus { get; set; } = new List<EOptionMenu>();
        [NotMapped]
        public virtual ICollection<EGroupOption> GroupOptions { get; set; } = new List<EGroupOption>();

    }
}
