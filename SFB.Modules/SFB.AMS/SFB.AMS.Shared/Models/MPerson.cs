
using System.ComponentModel.DataAnnotations;

namespace SFB.AMS.Shared.Models
{
    public class MPerson
    {
        [Key]
        [Required]
        public int NroPerson { get; set; } = 0;

        [Required, MaxLength(30)]
        public string FirstName { get; set; } = null!;

        [MaxLength(30)]
        public string? LastName { get; set; }

        [Required]
        public DateTime DateBirth { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
    }
}
