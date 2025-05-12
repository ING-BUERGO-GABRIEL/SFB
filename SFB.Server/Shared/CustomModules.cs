using System.Reflection;

namespace SFB.Server.Shared
{
    public static class CustomModules
    {
        public static IEnumerable<Assembly> GetBackendModules()
        {
            return new List<Assembly>
            {
                Assembly.Load("SFB.AMS.Backend"),
                Assembly.Load("SFB.CMS.Backend"),
                //Assembly.Load("SFB.TAM.Backend")
            };
        }

    }
}
