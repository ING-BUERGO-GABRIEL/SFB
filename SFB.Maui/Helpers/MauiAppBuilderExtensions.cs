using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;

namespace SFB.Maui.Helpers;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder AddSfbFonts(this MauiAppBuilder builder)
    {
        builder.ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        return builder;
    }

    public static MauiAppBuilder AddSfbHybridWebViewAndroid(this MauiAppBuilder builder)
    {
#if ANDROID
        Microsoft.Maui.Handlers.HybridWebViewHandler.Mapper.AppendToMapping("AllowMixedContent", (handler, view) =>
        {
            if (handler.PlatformView is global::Android.Webkit.WebView wv)
                wv.Settings.MixedContentMode = global::Android.Webkit.MixedContentHandling.AlwaysAllow;
        });
#endif
        return builder;
    }

    public static MauiAppBuilder AddSfbDebugLogging(this MauiAppBuilder builder)
    {
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder;
    }
}
