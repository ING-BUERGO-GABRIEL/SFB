using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace SFB.Maui.Platforms.Windows;

public static class WebView2Helpers
{
    public static async Task Configure(WebView2 wv2)
    {
        if (wv2.CoreWebView2 != null) return;

        var opts = new CoreWebView2EnvironmentOptions
        {
            AdditionalBrowserArguments = "--allow-running-insecure-content "
        };

        var env = await CoreWebView2Environment
                 .CreateWithOptionsAsync(browserExecutableFolder: null, 
                 userDataFolder: null, options: opts)
                 .AsTask();

#if DEBUG
        // Registrar el evento ANTES de inicializar
        wv2.CoreWebView2Initialized += (_, __) =>
        {
            wv2.CoreWebView2?.OpenDevToolsWindow();
        };
#endif

        await wv2.EnsureCoreWebView2Async(env);
      
    }
}
