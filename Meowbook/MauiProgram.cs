using Microsoft.Extensions.Logging;
using Meowbook.Services;
using Meowbook.ViewModels;
using Meowbook.Views;

namespace Meowbook
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("InterRegular.ttf", "InterRegular");
                    fonts.AddFont("PoppinsBold.ttf", "PoppinsBold");
                    fonts.AddFont("PoppinsMedium.ttf", "PoppinsMedium");
                    fonts.AddFont("PoppinsRegular.ttf", "PoppinsRegular");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ── Services ──────────────────────────────────────────────────
            builder.Services.AddSingleton<ApiService>();

            // ── ViewModels ────────────────────────────────────────────────
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<PostViewModel>();

            // ── Views / Pages ─────────────────────────────────────────────
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<LandingPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ForgotPassword>();
            builder.Services.AddTransient<RegistrationPage>();
            // HomePage gets HomeViewModel injected via constructor
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<MenuPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}