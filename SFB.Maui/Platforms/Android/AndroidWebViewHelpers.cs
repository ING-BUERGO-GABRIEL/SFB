using Android.Webkit;

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
    }
}
