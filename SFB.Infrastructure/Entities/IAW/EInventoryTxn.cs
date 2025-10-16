using SFB.Infrastructure.Entities.CMS;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_InventoryTxn")]
    public class EInventoryTxn : Auditable
    {
        [Key,Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TxnId { get; set; }

        public int? TxnOrigin { get; set; }

        [Required, MaxLength(3)]
        public string ModOrigin { get; set; }

        public int? WarehouseOri { get; set; }

        public int? WareHouseDest { get; set; }

        [NotMapped]
        public virtual EWarehouse? WarehouseOrigin { get; set; }

        [NotMapped]
        public virtual EWarehouse? WarehouseDest { get; set; }

        [Required]
        public ICollection<EInvDetail>? InvDetails { get; set; }

    }
}
