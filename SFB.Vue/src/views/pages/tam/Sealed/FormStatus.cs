
namespace SGD.TAM.Shared.Sealed
{
    public sealed class FormStatus
    {
        public string Code { get; }
        public string Description { get; }

        private FormStatus(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly FormStatus Item1 = new FormStatus("PEN", "PENDIENTE");
        public static readonly FormStatus Item2 = new FormStatus("UTI", "UTILIZADO");
        public static readonly FormStatus Item3 = new FormStatus("DES", "DESCARTADO");

        public static IEnumerable<FormStatus> List() => new[] { Item1, Item2, Item3 };

        public static FormStatus FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código no válido: {code}");
    }
}
