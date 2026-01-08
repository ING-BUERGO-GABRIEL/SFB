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
        var message = e.Message;

#if ANDROID
        if (message == "request-camera-permission")
        {
            // Ejecutar la lógica en el hilo principal para evitar PermissionException
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var granted = await EnsureCameraPermissionAsync();

                if (!granted)
                {
                    var shouldShow = Permissions.ShouldShowRationale<Permissions.Camera>();

                    if (!shouldShow)
                    {
                        var open = await DisplayAlert(
                            "Permiso de cámara",
                            "La aplicación necesita acceso a la cámara. Habilítalo en los ajustes de la aplicación.",
                            "Abrir ajustes",
                            "Cancelar");

                        if (open)
                        {
                            AppInfo.ShowSettingsUI();
                        }
                    }
                    else
                    {
                        await DisplayAlert(
                            "Permiso denegado",
                            "La aplicación requiere permiso para usar la cámara. Por favor acepta el permiso cuando se te solicite.",
                            "Entendido");
                    }
                }

                // Si necesitas notificar a la web, puedes ejecutar JS aquí:
                // await Hybrid.EvaluateJavaScriptAsync($"window.__onPermissionResult && window.__onPermissionResult({granted.ToString().ToLower()});");
            });
        }
#endif
    }

    public async Task<bool> EnsureCameraPermissionAsync()
    {
        // Ejecuta la comprobación/solicitud en el hilo principal y devuelve el resultado
        var status = await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var s = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (s != PermissionStatus.Granted)
                s = await Permissions.RequestAsync<Permissions.Camera>();

            return s;
        });

        return status == PermissionStatus.Granted;
    }

}
