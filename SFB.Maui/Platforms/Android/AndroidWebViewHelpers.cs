using Android.OS;
using Android.Webkit;
using Android.Content;

namespace SFB.Maui.Platforms.Android;

public static class AndroidWebViewHelpers
{
    public static void Configure(global::Android.Webkit.WebView wv)
    {
        if (wv == null) return;

        var s = wv.Settings;
        if (s == null) return;

        s.MixedContentMode = MixedContentHandling.AlwaysAllow;
        s.JavaScriptEnabled = true;
        s.DomStorageEnabled = true;
        s.AllowContentAccess = true;
        s.AllowFileAccess = true;
        s.JavaScriptCanOpenWindowsAutomatically = true;
        s.MediaPlaybackRequiresUserGesture = false;
        s.SetSupportMultipleWindows(true);

        // Enable debugging in debug builds
#if DEBUG
        global::Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif

        // Attach a WebChromeClient to handle getUserMedia permission requests
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            wv.SetWebChromeClient(new WebChromeClientForPermissions());
        }
    }

    class WebChromeClientForPermissions : WebChromeClient
    {
        public override void OnPermissionRequest(PermissionRequest request)
        {
            try
            {
                // Grant the requested resources (video/audio) for the WebView
                request.Grant(request.GetResources());
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnPermissionRequest error: {ex}");
            }
        }

        public override void OnPermissionRequestCanceled(PermissionRequest request)
        {
            System.Diagnostics.Debug.WriteLine("OnPermissionRequestCanceled");
        }
    }
}
