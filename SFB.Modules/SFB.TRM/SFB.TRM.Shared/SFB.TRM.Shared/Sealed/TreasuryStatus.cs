

namespace SFB.TRM.Shared.Sealed
{
    public sealed class TreasuryStatus
    {
        public string Code { get; }
        public string Name { get; }

        private TreasuryStatus(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly TreasuryStatus Pendiente = new("PEN", "Pendiente");
        public static readonly TreasuryStatus Aprobado = new("APR", "Aprobado");
        public static readonly TreasuryStatus Anulado = new("ANU", "Anulado");

        public static IEnumerable<TreasuryStatus> List() => new[] { Pendiente, Aprobado, Anulado };

        public static TreasuryStatus FromCode(string code) =>
            List().SingleOrDefault(x => x.Code == code) ?? throw new Exception("Status de tesorería inválido");
    }

}
