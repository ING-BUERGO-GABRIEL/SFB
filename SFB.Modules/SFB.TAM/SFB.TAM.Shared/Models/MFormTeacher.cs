using SFB.TAM.Shared.Sealed;
using System.ComponentModel.DataAnnotations;

namespace SFB.TAM.Shared.Models
{
    public class MFormTeacher
    {
        [Required]
        public int NroForm { get; set; } = 0;

        [Required]
        public int? PhoneNumber { get; set; }

        [Required]
        public string ProposalTitle { get; set; } = string.Empty;

        [Required]
        public string CodScope { get; set; } = string.Empty;

        [Required]
        public string CodModality { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un área de saberes y conocimientos")]
        public int? CodArea { get; set; }

        public string? CodAreaString {
            get => CodArea?.ToString() ?? null;
            set
            {
                if (int.TryParse(value, out var result))
                    CodArea = result == 0 ? null : result;
            }
        }

        [Required(ErrorMessage = "Debe seleccionar un Año de Escolaridad")]
        public int? CodSchoolYear { get; set; }

        [Required(ErrorMessage = "Debe escribir una materia")]
        public string Subject { get; set; }

        [Required]
        public string NameTeache { get; set; } = string.Empty;

        [Required]
        public string NameSchool { get; set; } = string.Empty;

        [Required]
        public string CodDepartment { get; set; } = string.Empty;

        [Required]
        public string Locality { get; set; } = string.Empty;

        [Required]
        public string CodStatus { get; set; } = "PEN";

        public virtual string? NameDepartment => Department.FromCode(CodDepartment).Description;
        public virtual string? NameScope => Scope.FromCode(CodScope).Description;
        public virtual string? NameModality => Modality.FromCode(CodModality).Description;
        public virtual string? NameSchoolYear => SchoolYear.FromCode(CodSchoolYear.Value).Description;
        public virtual string? NameArea => Area.FromCode(CodArea.Value).Description;
    }
}
