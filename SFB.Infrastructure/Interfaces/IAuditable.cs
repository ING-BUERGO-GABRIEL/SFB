using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Interfaces
{
    public interface IAuditable
    {
        string UserReg { get; set; }
        DateTime DateReg { get; set; }
        string? UserUpd { get; set; }
        DateTime? DateUpd { get; set; }
    }
}
