
namespace SGD.TAM.Shared.Sealed
{
    public sealed class Modality
    {
        public string Code { get; }
        public string Description { get; }

        private Modality(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Modality Regular = new Modality("REC", "Regular");
        public static readonly Modality Alternativa = new Modality("ALT", "Alternativa");
        public static readonly Modality Especial = new Modality("ESP", "Especial");
        public static readonly Modality Administrativo = new Modality("ADM", "Administrativo");

        public static IEnumerable<Modality> List() => new[] { Regular, Alternativa, Especial,Administrativo };

        public static Modality FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código de modalidad no válido: {code}");
    }
}
