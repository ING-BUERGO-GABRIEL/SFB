using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.CMS
{
    public class SettingsBase<E> : Auditable where E : class 
    {
        [Key]
        [Required]
        [MaxLength(10)]
        [Display(Name = "Id. Unico")]
        public string SettingId { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Empresa")]
        public string CompanyCode { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Sucursal")]
        public string BranchCode { get; set; }
        [MaxLength(10)]
        [Display(Name = "Grupo")]
        public string SettingsGroup { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Código")]
        public string ConfigCode { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string NameCode { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Descripción")]
        public string? DesCode { get; set; }
        [MaxLength(500)]
        [Display(Name = "Valor")]
        public string ValueCode { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Tipo")]
        public string ConfigType { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Origen")]
        public string OriginConfig { get; set; }
        [Display(Name = "Orden")]
        public short Order { get; set; }
        [Required]
        [MaxLength(10)]
        [Display(Name = "Estado")]
        public string StatusCode { get; set; }

        public virtual E Group { get; set; }
        [InverseProperty("Group")]
        public virtual ICollection<E> Settings { get; set; }
    }
}
