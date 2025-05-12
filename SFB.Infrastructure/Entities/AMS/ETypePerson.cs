using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.AMS
{
    public class ETypePerson
    {
        [Key]
        [Required]
        public int NroPerson { get; set; }

        [Key]
        [Required]
        public string Type { get; set; }

        [Required]
        public bool Status { get; set; } = true;
    }
}
