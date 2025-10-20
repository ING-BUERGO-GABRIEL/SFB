using SFB.Infrastructure.Entities.CMS;
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

        [Required, MaxLength(3)]
        public string Type { get; set; }

        public int? WarehouseOriginId { get; set; }
        public int? WarehouseDestId { get; set; }

        [ForeignKey(nameof(WarehouseOriginId))]
        public virtual EWarehouse? WarehouseOrigin { get; set; }

        [ForeignKey(nameof(WarehouseDestId))]
        public virtual EWarehouse? WarehouseDest { get; set; }


        [Required, MaxLength(3)]
        public string StatusCode { get; set; }

        [Required]
        public bool Delete { get; set; }

        public virtual ICollection<EInvDetail>? InvDetails { get; set; } = new List<EInvDetail>();

    }
}
