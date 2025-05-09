using SGD.Infrastructure.Helper;

namespace SGD.Infrastructure.Models
{
    public class DBConfiguration
    {
        public NameProvider NameProvider { get; set; }
        public Version Version { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            switch (NameProvider)
            {
                case NameProvider.MYSQL:
                    return $"Server={Server};Database={Database};User={User};Password={Password};";
                default:
                    throw new Exception("NameProvider not Suport");
            }

        }
    }
}
