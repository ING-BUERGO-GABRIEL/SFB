
namespace SFB.TAM.Shared.Sealed
{
    public sealed class Area
    {
        public int Code { get; }
        public string CodModality { get; }
        public string CodSpecialty { get; }
        public string Description { get; }

        private Area(int code, string codModality, string codSpecialty, string description)
        {
            Code = code;
            CodSpecialty = codSpecialty;
            CodModality = codModality;
            Description = description;
        }

        public static readonly Area Alternativa = new Area(32, "ALT","ALT", "ALTERNATIVO");
        public static readonly Area Especial = new Area(33, "ESP","ESP", "ESPECIAL");
        public static readonly Area Inicial1 = new Area(1, "REC","INI", "INICIAL");
        public static readonly Area Primaria1 = new Area(5, "REC", "PRI", "PRIMARIA");
        public static readonly Area Secundaria1 = new Area(14, "REC", "SEC", "SECUNDARIA");
        public static readonly Area Primaria2 = new Area(6, "ADM", "PRI", "PRIMARIA");
        public static readonly Area Secundaria2 = new Area(7, "ADM", "SEC", "SECUNDARIA");

        public static IEnumerable<Area> List() => new[] {
                    Alternativa, Especial, Inicial1, 
                    Primaria1,Primaria2,
                    Secundaria1,Secundaria2,
                };

        public static Area FromCode(int code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new ArgumentException($"Código de modalidad no válido: {code}");
    }
}
