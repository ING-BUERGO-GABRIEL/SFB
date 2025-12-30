using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFB.SOM.Shared.Sealed
{
    public sealed class SalesStatus
    {
        public string Code { get; }
        public string Name { get; }

        private SalesStatus(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly SalesStatus Activo = new SalesStatus("ACT", "Activo");
        public static readonly SalesStatus Pendiente = new SalesStatus("PEN", "Pendiente");
        public static readonly SalesStatus Anulado = new SalesStatus("ANU", "Anulado");

        public static IEnumerable<SalesStatus> List() => new[] { Activo, Anulado, Pendiente };

        public static SalesStatus FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new Exception("Codigo Status Venta Error");
    }
}
