
namespace SFB.PCM.Shared.Sealed
{
    public sealed class PurchaseStatus
    {
        public string Code { get; }
        public string Name { get; }

        private PurchaseStatus(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly PurchaseStatus Activo = new PurchaseStatus("ACT", "Activo");
        public static readonly PurchaseStatus Pendiente = new PurchaseStatus("PEN", "Pendiente");
        public static readonly PurchaseStatus Anulado = new PurchaseStatus("ANU", "Anulado");

        public static IEnumerable<PurchaseStatus> List() => new[] { Activo, Anulado, Pendiente };

        public static PurchaseStatus FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new Exception("Codigo Status Error");
    }
}
