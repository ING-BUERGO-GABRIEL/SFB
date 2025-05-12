using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.CMS.Shared.Models
{
    public class MOptionMenu
    {
        [MaxLength(20)]
        [Required]
        public string CodOption { get; set; }

        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Icon { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [MaxLength(20)]
        public string? CodGruOption { get; set; }

        [MaxLength(100)]
        public string? Route { get; set; }

        public virtual List<MOptionMenu>? OptionMenus { get; set; }

    }
}
