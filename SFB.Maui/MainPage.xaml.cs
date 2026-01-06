#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using SFB.Maui.Platforms.Windows;
#endif

#if ANDROID
using SFB.Maui.Platforms.Android;
#endif

namespace SFB.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        Hybrid.HandlerChanged += async (_, __) =>
        {
#if ANDROID
            if (Hybrid.Handler?.PlatformView is Android.Webkit.WebView wv)
                AndroidWebViewHelpers.Configure(wv);
#elif WINDOWS
            if (Hybrid.Handler?.PlatformView is WebView2 wv2)
                await WebView2Helpers.Configure(wv2);
#endif
        };
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
