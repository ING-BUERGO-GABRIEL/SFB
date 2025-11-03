
namespace SFB.IAW.Shared.Sealed
{
    public sealed class InvStatus
    {
        public string Code { get; }
        public string Name { get; }

        private InvStatus(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly InvStatus Activo = new InvStatus("ACT", "Activo");
        public static readonly InvStatus Pendiente = new InvStatus("PEN", "Pendiente");
        public static readonly InvStatus Anulado = new InvStatus("ANU", "Anulado");

        public static IEnumerable<InvStatus> List() => new[] { Activo, Anulado, Pendiente };

        public static InvStatus FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new Exception("Codigo Status Error");
    }

}
