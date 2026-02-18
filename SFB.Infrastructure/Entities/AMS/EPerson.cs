
using SFB.Infrastructure.Entities.TAM;
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


        [Required, MaxLength(30)]
        public string FirstName { get; set; } = null!;

        [MaxLength(30)] 
        public string? LastName { get; set; }

        [Required]
        public DateTime DateBirth { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [NotMapped]
        public virtual ICollection<ETeacherTask>? TeacherTasksAssigned { get; set; }

        [NotMapped]
        public virtual ICollection<ETeacherTask>? TeacherTasksReturned { get; set; }

        [NotMapped]
        public virtual ICollection<EUser> Users { get; set; } = new List<EUser>();

    }
}
