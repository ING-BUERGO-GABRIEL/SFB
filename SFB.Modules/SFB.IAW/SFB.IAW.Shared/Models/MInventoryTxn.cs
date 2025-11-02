using SFB.Infrastructure.Entities.IAW.Sealed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.IAW.Shared.Models
{
    public class MInventoryTxn
    {
        [Key, Required]
        public int TxnId { get; set; }

        public int? TxnOrigin { get; set; }

        [Required, MaxLength(3)]
        public string ModOrigin { get; set; }

        [Required, MaxLength(3)]
        public string Type { get; set; }

        public string NameType => InvType.FromCode(Type).Name;

        public int? WarehouseOriginId { get; set; }
        public int? WarehouseDestId { get; set; }

        [Required, MaxLength(3)]
        public string StatusCode { get; set; }

        public string NameStatus => InvStatus.FromCode(StatusCode).Name;

        public DateTime? DateReg { get; set; }

        public virtual ICollection<MInvDetail>? InvDetails { get; set; } = new List<MInvDetail>();
    }
}
