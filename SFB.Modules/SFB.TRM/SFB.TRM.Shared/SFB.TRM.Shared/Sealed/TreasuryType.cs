

namespace SFB.TRM.Shared.Sealed
{
    public sealed class TreasuryType
    {
        public string Code { get; }
        public string Name { get; }

        private TreasuryType(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly TreasuryType Ingreso = new("ING", "Ingreso");
        public static readonly TreasuryType Egreso = new("EGR", "Egreso");
        public static readonly TreasuryType Traslado = new("TRA", "Traslado");

        public static IEnumerable<TreasuryType> List() => new[] { Ingreso, Egreso, Traslado };

        public static TreasuryType FromCode(string code) =>
            List().SingleOrDefault(x => x.Code == code) ?? throw new Exception("Tipo de tesorería inválido");
    }
}
