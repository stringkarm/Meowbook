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

            // 1. Register Services
            builder.Services.AddSingleton<ApiService>();

            // 2. Register ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<PostViewModel>();

            // 3. Register Views (Pages)
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ForgotPassword>();
            //builder.Services.AddTransient<ForgotPasswordViewModel>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>(); ;
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<MainTabbedPage>();
            builder.Services.AddTransient<MenuPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}