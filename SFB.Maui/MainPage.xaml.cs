#if WINDOWS
using Microsoft.UI.Xaml.Controls;
using SFB.Maui.Platforms.Windows;
#endif

#if ANDROID
using SFB.Maui.Platforms.Android;
#endif

using Microsoft.Maui.ApplicationModel;

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

    private async void RawMessageReceived(object sender, HybridWebView.HybridWebViewRawMessageReceivedEventArgs e)
    {
        var action = e.Message;

        try
        {
            if (action == "request-camera-permission")
            {
                var granted = false;
#if ANDROID
                granted = await EnsureCameraPermissionAsync();
#endif
                await Hybrid.EvaluateJavaScriptAsync($"window.__onPermissionResult && window.__onPermissionResult({granted.ToString().ToLower()});");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RawMessageReceived error: {ex}");
        }
    }

    public async Task<bool> EnsureCameraPermissionAsync()
    {
        return await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var status = await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var s = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (s != PermissionStatus.Granted)
                    s = await Permissions.RequestAsync<Permissions.Camera>();

                return s;
            });

            return status == PermissionStatus.Granted;
        });

    }

}
