using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.CMS.Shared.Models
{
    public class MModule
    {
        [Key]
        [MaxLength(3)]
        [Required]
        public string CodModule { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Icon { get; set; }

        public List<MOptionMenu>? OptionMenus { get; set; } = new List<MOptionMenu>();
    }
}
