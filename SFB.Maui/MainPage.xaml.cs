#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
#endif

#if ANDROID

using Microsoft.Maui.Handlers;

#endif

namespace SFB.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Hybrid.SetInvokeJavaScriptTarget(this);
#if WINDOWS
        Hybrid.HandlerChanged += (_, __) =>
        {
            // PlatformView en Windows es WebView2 (Microsoft.UI.Xaml.Controls.WebView2)
            if (Hybrid.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.WebView2 wv2)
            {
                // CoreWebView2 puede tardar en inicializar
                wv2.CoreWebView2Initialized += (_, __) =>
                {
                    wv2.CoreWebView2.OpenDevToolsWindow(); // abre DevTools :contentReference[oaicite:2]{index=2}
                };
                wv2.CoreWebView2Initialized += (_, __) =>
            {
                var core = wv2.CoreWebView2;

#if DEBUG
                // 1) Aceptar certificado inválido (DEV ONLY)
                core.ServerCertificateErrorDetected += (s, e) =>
                {
                    e.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
                };

                // 2) Abrir DevTools automáticamente (DEV ONLY)
                core.OpenDevToolsWindow();
#endif
            };
            }



        };


#endif

#if ANDROID
        Hybrid.HandlerChanged += (_, __) =>
        {
            // OJO: aquí necesitamos el tipo concreto WebViewHandler
            if (Hybrid.Handler is WebViewHandler handler &&
                handler.PlatformView is Android.Webkit.WebView webView)
            {
#if DEBUG
                webView.SetWebViewClient(new SFB.Maui.Platforms.Android.UnsafeMauiWebViewClient(handler));
#endif
            }
        };
#endif

    }

    public async Task<bool> EnsureCameraPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        return status == PermissionStatus.Granted;
    }
}
