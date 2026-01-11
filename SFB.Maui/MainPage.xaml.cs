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
                // Ensure all WebView interactions happen on the main thread
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var granted = false;
#if ANDROID
                    granted = await EnsureCameraPermissionAsync();
#endif
                    var script = $"window.__onPermissionResult && window.__onPermissionResult({granted.ToString().ToLower()});";
                    try
                    {
                        await Hybrid.EvaluateJavaScriptAsync(script);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"EvaluateJavaScriptAsync error: {ex}");
                    }
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RawMessageReceived error: {ex}");
        }
    }

    public async Task<bool> EnsureCameraPermissionAsync()
    {
        // This method must be called from the main thread.
        var s = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (s != PermissionStatus.Granted)
            s = await Permissions.RequestAsync<Permissions.Camera>();

        return s == PermissionStatus.Granted;

    }

}
