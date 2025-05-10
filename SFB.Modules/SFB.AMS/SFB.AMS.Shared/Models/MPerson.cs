
using System.ComponentModel.DataAnnotations;

namespace SGD.AMS.Shared.Models
{
    public class MPerson
    {
        [Key]
        [Required]
        public int NroPerson { get; set; } = 0;

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string? FirstLastName { get; set; }

        [MaxLength(30)]
        public string? SecondLastName { get; set; }

        [Required]
        public DateTime DateBirth { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
    }
}
