
namespace SGD.TAM.Shared.Sealed
{
    public sealed class Department
    {
        public string Code { get; }
        public string Description { get; }

        private Department(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Department SantaCruz = new Department("SC", "Santa Cruz");
        public static readonly Department Cochabamba = new Department("CB", "Cochabamba");
        public static readonly Department Tarija = new Department("TJ", "Tarija");
        public static readonly Department LaPaz = new Department("LP", "La Paz");
        public static readonly Department Potosi = new Department("PT", "Potosí");
        public static readonly Department Oruro = new Department("OR", "Oruro");
        public static readonly Department Chuquisaca = new Department("CH", "Chuquisaca");
        public static readonly Department Beni = new Department("BN", "Beni");
        public static readonly Department Pando = new Department("PD", "Pando");

        public static IEnumerable<Department> List() => new[]
        {            
            SantaCruz,   Cochabamba, Tarija, LaPaz, Potosi,  Oruro,    Chuquisaca,  Beni,    Pando
        };
        public static Department FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código de modalidad no válido: {code}");
    }
}
