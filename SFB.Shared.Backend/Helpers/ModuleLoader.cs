using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SFB.Shared.Backend.Helpers
{
    public static class ModuleLoader
    {
        public static void LoadBackendModules(IEnumerable<Assembly> modules, IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApplicationPartManager(partManager =>
                {
                    foreach (var module in modules)
                    {
                        partManager.ApplicationParts.Add(new AssemblyPart(module));
                    }
                });
        }

    }
}
