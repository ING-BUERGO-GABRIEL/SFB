using SFB.AMS.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace SFB.SOM.Shared.Models
{
    public class MCustomer
    {
        [Key, Required]
        public int CustomerId { get; set; }

        [Required]
        public int NroPerson { get; set; }
        public virtual MPerson? Person { get; set; }
    }
}
