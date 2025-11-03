

namespace SFB.IAW.Shared.Sealed
{
    public sealed class InvType
    {
        public string Code { get; }
        public string Name { get; }

        private InvType(string type, string name)
        {
            Code = type;
            Name = name;
        }

        public static readonly InvType Ingreso = new InvType("ING", "Ingreso");
        public static readonly InvType Salida = new InvType("SAL", "Salida");
        public static readonly InvType Traspaso = new InvType("TRA", "Traspaso");
        public static readonly InvType TxnInicial = new InvType("INI", "Txn. Inicial");

        public static IEnumerable<InvType> List() => new[] { Ingreso, Salida, Traspaso, TxnInicial };

        public static InvType FromCode(string type) =>
            List().SingleOrDefault(m => m.Code == type) ?? throw new Exception("Tipo de transaccion erroneo");
    }

}
