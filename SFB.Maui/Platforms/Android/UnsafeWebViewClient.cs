#if ANDROID
using Android.Net.Http;
using Android.Webkit;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace SFB.Maui.Platforms.Android;

public class UnsafeMauiWebViewClient : MauiWebViewClient
{
    public UnsafeMauiWebViewClient(WebViewHandler handler) : base(handler) { }

    public override void OnReceivedSslError(
        global::Android.Webkit.WebView view,
        SslErrorHandler handler,
        SslError error)
    {
#if DEBUG
        handler.Proceed(); // DEV ONLY: acepta cualquier certificado
#else
        base.OnReceivedSslError(view, handler, error);
#endif
    }
}
#endif
