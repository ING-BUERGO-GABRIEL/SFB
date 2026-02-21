
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.TAM.Shared.Models
{
    public class MTeacherTask
    {
        [Key]
        [Required]
        public int NroTask { get; set; } = 0;
        [Required]
        [MaxLength(20)]
        public string NameContact { get; set; }
        [Required]
        public int NroFromTeacher { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio total debe ser mayor a 0.")]
        public decimal PriceTotal { get; set; }

        [Required(ErrorMessage = "El monto pagado es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto pagado no puede ser negativo.")]
        public decimal AmountPaid { get; set; }


        [Required]
        [MaxLength(3)]
        public string CodStatus { get; set; }


        [Required]
        [MaxLength(500)]
        public string UrlDocument { get; set; }


        [Required]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public int NroPersonReturn { get; set; }
        public int? NroPersonAssigned { get; set; }

        [NotMapped]
        public virtual MFormTeacher? FormTeacher { get; set; }
        [NotMapped]
        public virtual DateTime? DateReg { get; set; }
    }
}
