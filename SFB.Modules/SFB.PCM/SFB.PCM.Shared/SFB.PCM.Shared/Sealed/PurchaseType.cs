
namespace SFB.PCM.Shared.Sealed
{
    public sealed class PurchaseType
    {
        public string Code { get; }
        public string Name { get; }

        private PurchaseType(string type, string name)
        {
            Code = type;
            Name = name;
        }

        public static readonly PurchaseType Ingreso = new PurchaseType("ING", "Ingreso");
        public static readonly PurchaseType TxnInicial = new PurchaseType("INI", "Txn. Inicial");

        public static IEnumerable<PurchaseType> List() => new[] { Ingreso, TxnInicial };

        public static PurchaseType FromCode(string type) =>
            List().SingleOrDefault(m => m.Code == type) ?? throw new Exception("Tipo de transaccion erroneo");
    }
}
