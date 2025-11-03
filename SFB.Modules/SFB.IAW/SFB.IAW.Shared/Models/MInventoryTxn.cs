using SFB.IAW.Shared.Sealed;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.IAW.Shared.Models
{
    public class MInventoryTxn
    {
        [Key, Required]
        public int TxnId { get; set; }

        [Required, MaxLength(3)]
        public string ModOrigin { get; set; }
        public int? TxnOrigin { get; set; }

        [Required, MaxLength(3)]
        public string Type { get; set; }
        public string NameType => InvType.FromCode(Type).Name;

        public int? WarehouseOriginId { get; set; }
        public int? WarehouseDestId { get; set; }
        public virtual MWarehouse? WarehouseOrigin { get; set; }
        public virtual MWarehouse? WarehouseDest { get; set; }


        [Required, MaxLength(3)]
        public string StatusCode { get; set; }
        public InvStatus Status => InvStatus.FromCode(StatusCode);

        public DateTime? DateReg { get; set; }

        public virtual ICollection<MInvDetail>? InvDetails { get; set; } = new List<MInvDetail>();
    }
}
