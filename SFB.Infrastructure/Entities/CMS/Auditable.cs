using System.ComponentModel.DataAnnotations;
using SGD.Infrastructure.Interfaces;

namespace SGD.Infrastructure.Entities.CMS
{
    public class Auditable : IAuditable
    {
        [Required]
        [MaxLength(50)]
        public string UserReg { get; set; }

        [Required]
        public DateTime DateReg { get; set; }

        [MaxLength(50)]
        public string? UserUpd { get; set; }
        public DateTime? DateUpd { get; set; }
    }
}
