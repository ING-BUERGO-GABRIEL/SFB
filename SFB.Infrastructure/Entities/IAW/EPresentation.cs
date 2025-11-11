using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations;

namespace SFB.Infrastructure.Entities.IAW
{
    public class EPresentation :Auditable
    {
        [Key]
        [Required,MaxLength(3)]
        public string Code { get; set; }

        [Required ,MaxLength(30)]
        public string Name { get; set; } 
    }
}
