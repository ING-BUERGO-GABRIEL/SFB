
namespace SFB.TAM.Shared.Sealed
{
    public sealed class Specialty
    {
        public string Code { get; }
        public string CodLevel { get; }
        public string Description { get; }

        private Specialty(int code, string codLevel, string description)
        {
            Code = code.ToString();
            CodLevel = codLevel;
            Description = description;
        }

        public static readonly Specialty Altenatica = new Specialty(1, "ALT", "ALTERNATIVA");
        public static readonly Specialty Especial = new Specialty(2, "ESP", "ESPECIAL");
        public static readonly Specialty Inicia = new Specialty(3, "INI", "DOCENTE AULA (INICIAL)");
        public static readonly Specialty Primaria1 = new Specialty(4, "PRI", "DOCENTE AULA (PRIMARIA)");
        public static readonly Specialty Primaria2 = new Specialty(5, "PRI", "VALORES, ESPIRITUALIDAD Y RELIGIONES (PRIMARIA-SECUNDARIA)");
        public static readonly Specialty Primaria3 = new Specialty(6, "PRI", "EDUCACION FISICA Y DEPORTES (PRIMARIA-SECUNDARIA)");
        public static readonly Specialty Primaria4 = new Specialty(7, "PRI", "TECNICA TECNOLOGICA, AREAS TECNICAS (PRIMARIA-SECUNDARIA)");
        public static readonly Specialty Primaria5 = new Specialty(8, "PRI", "EDUCACION MUSICAL (PRIMARIA-SECUNDARIA)");
        public static readonly Specialty Secundaria1 = new Specialty(9, "SEC", "FILOSOFIA-PSICOLOGIA (SECUNDARIA)");
        public static readonly Specialty Secundaria2 = new Specialty(10, "SEC", "COMUNICACION Y LENGUAJE (SECUNDARIA)");
        public static readonly Specialty Secundaria3 = new Specialty(11, "SEC", "ESTUDIOS SOCIALES, HISTORIA, GEOGRAFIA, EDUCACION CIVICA (SECUNDARIA)");
        public static readonly Specialty Secundaria4 = new Specialty(12, "SEC", "IDIOMA INGLES (SECUNDARIA)");
        public static readonly Specialty Secundaria5 = new Specialty(13, "SEC", "ARTES PLASTICAS (SECUNDARIA)");
        public static readonly Specialty Secundaria6 = new Specialty(14, "SEC", "CIENCIAS NATURALES, BIOLOGIA (SECUNDARIA)");
        public static readonly Specialty Secundaria7 = new Specialty(15, "SEC", "FISICA (SECUNDARIA)");
        public static readonly Specialty Secundaria8 = new Specialty(16, "SEC", "TECNICA TECNOLOGICA, AREAS TECNICAS (PRIMARIA-SECUNDARIA)");
        public static readonly Specialty Secundaria9 = new Specialty(17, "SEC", "MATEMATICA (SECUNDARIA)");
        public static readonly Specialty Secundaria10 = new Specialty(18, "SEC", "QUIMICA (SECUNDARIA)");
        public static readonly Specialty Secundaria11 = new Specialty(19, "SEC", "FISICA - QUIMICA (SECUNDARIA)");
        public static readonly Specialty Secundaria12 = new Specialty(20, "SEC", "EDUCACION MUSICAL (PRIMARIA-SECUNDARIA)");

        public static IEnumerable<Specialty> List() => new[]
        {
            Altenatica,    Especial,    Inicia,    Primaria1,    Primaria2,    Primaria3,    Primaria4,    Primaria5,    Secundaria1,
            Secundaria2,    Secundaria3,    Secundaria4,    Secundaria5,    Secundaria6,    Secundaria7,    Secundaria8,
            Secundaria9,    Secundaria10,    Secundaria11,    Secundaria12
        };

        public static Specialty FromCode(string code) =>
                 List().SingleOrDefault(m => m.Code == code)
                 ?? throw new ArgumentException($"Código de especialidad no válido: {code}");

    }
}
