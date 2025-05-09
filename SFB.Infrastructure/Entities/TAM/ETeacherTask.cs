
using SGD.Infrastructure.Entities.AMS;
using SGD.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGD.Infrastructure.Entities.TAM
{
    public class ETeacherTask : Auditable
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroTask { get; set; }
        [Required]
        [MaxLength(20)]
        public string NameContact { get; set; }
        [Required]
        public int NroFromTeacher { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PriceTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public int NroPersonReturn { get; set; }
        public int? NroPersonAssigned { get; set; }

        [Required]
        [MaxLength(3)]
        public string CodStatus { get; set; }

        [Required]
        [MaxLength(500)]
        public string UrlDocument { get; set; }


        [Required]
        public DateTime DeliveryDate { get; set; }

        //Virtuales
        //Foreign key
        [NotMapped]
        [ForeignKey("NroPersonAssigned")]
        public virtual EPerson? PersonAssigned { get; set; }

        [NotMapped]
        [ForeignKey("NroPersonReturn")]
        public virtual EPerson? PersonReturn { get; set; }

        [ForeignKey("NroFromTeacher")]
        [NotMapped]
        public virtual EFormTeacher? FormTeacher { get; set; }
    }
}
