using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Infrastructure.Entities.IAW.Sealed
{
    public sealed class InvTxnType
    {
        public string Type { get; }
        public string Name { get; }

        private InvTxnType(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public static readonly InvTxnType Ingreso = new InvTxnType("ING", "Ingreso");
        public static readonly InvTxnType Salida = new InvTxnType("SAL", "Salida");
        public static readonly InvTxnType Traspaso = new InvTxnType("TRA", "Traspaso");

        public static IEnumerable<InvTxnType> List() => new[] { Ingreso, Salida, Traspaso };

        public static InvTxnType? FromCode(string type) =>
            List().SingleOrDefault(m => m.Type == type) ?? null;
    }

}
