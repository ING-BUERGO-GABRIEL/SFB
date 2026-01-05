#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
#endif

#if ANDROID

using Microsoft.Maui.Handlers;
using Android.Webkit;
#endif

namespace SFB.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

#if ANDROID
        Hybrid.HandlerChanged += (_, __) =>
        {
            if (Hybrid.Handler?.PlatformView is Android.Webkit.WebView wv)
            {
                // Permitir Mixed Content (HTTPS page -> HTTP API)
                wv.Settings.MixedContentMode = MixedContentHandling.AlwaysAllow;

                // Opcional (a veces necesario en SPAs)
                wv.Settings.JavaScriptEnabled = true;
                wv.Settings.DomStorageEnabled = true;
            }
        };
#endif

#if WINDOWS
        Hybrid.HandlerChanged += async  (_, __) =>
        {
            // PlatformView en Windows es WebView2 (Microsoft.UI.Xaml.Controls.WebView2)
            if (Hybrid.Handler?.PlatformView is WebView2 wv2)
            {          

                // CoreWebView2 puede tardar en inicializar
                wv2.CoreWebView2Initialized += (_, __) =>
                {
                    wv2.CoreWebView2.OpenDevToolsWindow(); // abre DevTools :contentReference[oaicite:2]{index=2}
                };                
           }            
        };
#endif

    }

    private void RawMessageReceived(object sender, HybridWebView.HybridWebViewRawMessageReceivedEventArgs e)
    {
        var message = e.Message;
    }

    public async Task<bool> EnsureCameraPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        return status == PermissionStatus.Granted;
    }

}
