
namespace SGD.TAM.Shared.Sealed
{
    public sealed class Scope
    {
        public string Code { get; }
        public string Description { get; }

        private Scope(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Scope Rural = new Scope("RUR", "Rural");
        public static readonly Scope Urbano = new Scope("URB", "Urbano");

        public static IEnumerable<Scope> List() => new[] { Rural, Urbano };

        public static Scope FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código de modalidad no válido: {code}");
    }
}
