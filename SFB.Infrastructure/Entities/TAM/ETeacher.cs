using SFB.Infrastructure.Entities.AMS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.TAM
{
    public class ETeacher
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroTeacher { get; set; }
        [Required]
        public int NroPerson { get; set; }

        public string Address { get; set; }

        //Virtuales
        [NotMapped]
        [ForeignKey("NroPerson")]
        public virtual EPerson Person { get; set; }

        [NotMapped]
        [ForeignKey("NroPerson")]
        public virtual ETypePerson TypePerson { get; set; }
    }
}
