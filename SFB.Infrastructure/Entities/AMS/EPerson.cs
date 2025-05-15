
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFB.Infrastructure.Entities.AMS
{
    public class EPerson
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NroPerson { get; set; }

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


        [NotMapped]
        public virtual ICollection<EUser> Users { get; set; } = new List<EUser>();

    }
}
