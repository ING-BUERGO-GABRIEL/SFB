
namespace SGD.TAM.Shared.Sealed
{
    public sealed class FTaskStatus
    {
        public string Code { get; }
        public string Description { get; }

        private FTaskStatus(string code, string description)
        {
            Code = code;
            Description = description;
        }

        //public static readonly FTaskStatus Item1 = new FTaskStatus("VER", "VERIFICACION PRIMER PAGO");
        public static readonly FTaskStatus Item2 = new FTaskStatus("PEN", "PENDIENTE DE INICIO");
        public static readonly FTaskStatus Item3 = new FTaskStatus("ELA", "EN ELABORACION");
        public static readonly FTaskStatus Item4 = new FTaskStatus("BIB", "FALTA BIBLIOGRAFIA");
        public static readonly FTaskStatus Item5 = new FTaskStatus("REV", "PARA REVISAR");
        public static readonly FTaskStatus Item6 = new FTaskStatus("PTE", "PARA ENTREGAR");
        public static readonly FTaskStatus Item7 = new FTaskStatus("NOT", "NOTIFICADO PARA ENTREGAR");
        public static readonly FTaskStatus Item8 = new FTaskStatus("ENT", "ENTREGADO");

        public static IEnumerable<FTaskStatus> List() => new[] {  Item2, Item3, Item4, Item5, Item6, Item7, Item8 };

        public static FTaskStatus FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código no válido: {code}");
    }
}
