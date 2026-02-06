
namespace SGD.TAM.Shared.Sealed
{
    public sealed class SchoolYear
    {
        public int Code { get; }
        public string Description { get; }

        private SchoolYear(int code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly SchoolYear Primero = new SchoolYear(1,"Primero");
        public static readonly SchoolYear Segundo = new SchoolYear(2, "Segundo");
        public static readonly SchoolYear Tercero = new SchoolYear(3, "Tercero");
        public static readonly SchoolYear Cuarto = new SchoolYear(4, "Cuarto");
        public static readonly SchoolYear Quinto = new SchoolYear(5, "Quinto");
        public static readonly SchoolYear Sexto = new SchoolYear(6,  "Sexto");

        public static IEnumerable<SchoolYear> List() => new[] {
                    Primero, Segundo, Tercero, Cuarto, Quinto, Sexto
                };

        public static SchoolYear FromCode(int code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código de modalidad no válido: {code}");
    }
}
