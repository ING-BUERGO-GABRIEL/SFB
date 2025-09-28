using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SFB.IAW.Shared.Models
{
    public class MWarehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string? Location { get; set; }

        [Required]
        public bool Status { get; set; } = true;
    }
}
