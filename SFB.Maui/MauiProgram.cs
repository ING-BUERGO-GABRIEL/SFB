using SFB.Maui.Helpers;

namespace SFB.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .AddSfbFonts()
            .AddSfbHybridWebViewAndroid()
            .AddSfbDebugLogging();

        builder.Services.AddHybridWebView();
        return builder.Build();
    }
}
