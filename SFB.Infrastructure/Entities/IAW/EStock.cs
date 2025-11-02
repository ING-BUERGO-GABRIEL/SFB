using SFB.Infrastructure.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SFB.Infrastructure.Entities.IAW
{
    [Table("IAW_Stock")]
    public class EStock : Auditable
    {

        [Key]
        [Required]
        public int NroProduct { get; set; }

        [ForeignKey(nameof(NroProduct))]
        public EProduct Product { get; set; }

        [Key]
        [Required]
        public int WarehouseId { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public EWarehouse Warehouse { get; set; }

        // Cantidad disponible física
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyOnHand { get; set; }

        // Cantidad comprometida (reservas en ventas, etc.)
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal QtyReserved { get; set; }

        // Punto de reorden opcional
        [Column(TypeName = "decimal(18,4)")]
        public decimal? ReorderPoint { get; set; }

    }
}
