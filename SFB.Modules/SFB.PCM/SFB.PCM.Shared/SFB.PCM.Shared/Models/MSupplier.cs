using System.ComponentModel.DataAnnotations;

namespace SFB.PCM.Shared.Models
{
    public class MSupplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, MaxLength(70)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }
    }
}
