using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.TAM
{
    public class EFormTeacher
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroForm { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public string ProposalTitle { get; set; } = string.Empty;

        [Required]
        public string CodScope { get; set; } = string.Empty;

        [Required]
        public string CodModality { get; set; } = string.Empty;

        [Required]
        public int CodArea { get; set; }

        [Required]
        public int CodSchoolYear { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string NameTeache { get; set; }

        [Required]
        public string NameSchool { get; set; }

        [Required]
        public string CodDepartment { get; set; }

        [Required]
        public string Locality { get; set; }

        [Required]
        public string CodStatus { get; set; }

        //Visualizaciones
        //Collection
        [NotMapped]
        public virtual ICollection<ETeacherTask>? TeacherTask { get; set; }
    }
}