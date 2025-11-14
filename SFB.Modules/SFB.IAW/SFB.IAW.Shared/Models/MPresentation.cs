using System.ComponentModel.DataAnnotations;

namespace SFB.IAW.Shared.Models
{
    public class MPresentation
    {
        [Key]
        [Required, MaxLength(3)]
        public string Code { get; set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }
    }
}
