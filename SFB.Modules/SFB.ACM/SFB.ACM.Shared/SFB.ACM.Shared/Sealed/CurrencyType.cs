
namespace SFB.ACM.Shared.Sealed
{
    public sealed class CurrencyType
    {
        public string Code { get; }
        public string Name { get; }

        private CurrencyType(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static readonly CurrencyType Boliviano = new CurrencyType("BOB", "Boliviano");
        public static readonly CurrencyType Dolar = new CurrencyType("USD", "Dolar");


        public static IEnumerable<CurrencyType> List() => new[] { Boliviano, Dolar};

        public static CurrencyType FromCode(string code) =>
            List().SingleOrDefault(m => m.Code == code) ?? throw new Exception("Codigo Status Error");
    }
}
