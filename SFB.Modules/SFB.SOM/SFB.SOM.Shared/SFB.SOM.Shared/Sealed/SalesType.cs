using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.SOM.Shared.Sealed
{
    public sealed class SalesType
    {
        public string Code { get; }
        public string Name { get; }

        private SalesType(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly SalesType Venta = new SalesType("VEN", "Venta");
        public static readonly SalesType Cotizacion = new SalesType("COT", "Cotizacion");
        public static readonly SalesType Reserva = new SalesType("RES", "Reserva");

        public static IEnumerable<SalesType> List() => new[] { Venta,Cotizacion,Reserva };

        public static SalesType FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new Exception("Tipo de transaccion de venta erroneo");
    }

}
