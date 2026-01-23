
namespace SFB.TRM.Shared.Sealed
{
    public sealed class PaymentMethod
    {
        public string Code { get; }
        public string Name { get; }

        private PaymentMethod(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly PaymentMethod Efectivo = new("CASH", "Efectivo");
        public static readonly PaymentMethod Tarjeta = new("CRD", "Tarjeta");
        public static readonly PaymentMethod QR = new("QR", "QR");
        public static readonly PaymentMethod Transfer = new("TRF", "Transferencia");

        public static IEnumerable<PaymentMethod> List() => new[] { Efectivo, Tarjeta, QR, Transfer };

        public static PaymentMethod FromCode(string code) =>
            List().SingleOrDefault(x => x.Code == code) ?? throw new Exception("Método de pago inválido");
    }
}
