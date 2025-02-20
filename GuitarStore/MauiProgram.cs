using GuitarStore.Services;
using GuitarStore.ViewModels;
using GuitarStore.Views;
using Microsoft.Extensions.Logging;


namespace GuitarStore
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<AuthenticationService>();
            
            builder.Services.AddTransient<GuitarViewModel>();
            builder.Services.AddTransient<GuitarPage>();
            builder.Services.AddTransient<AddGuitarPage>();
            builder.Services.AddTransient<AddGuitarViewModel>();
            builder.Services.AddTransient<AmpViewModel>();
            builder.Services.AddTransient<AmpPage>();
            builder.Services.AddTransient<AddAmpPage>();
            builder.Services.AddTransient<AddAmpViewModel>();
            builder.Services.AddTransient<PedalViewModel>();
            builder.Services.AddTransient<PedalPage>();
            builder.Services.AddTransient<AddPedalPage>();
            builder.Services.AddTransient<AddPedalViewModel>();
            builder.Services.AddTransient<AccessoryViewModel>();
            builder.Services.AddTransient<AccessoryPage>();
            builder.Services.AddTransient<AddAccessoryPage>();
            builder.Services.AddTransient<AddAccessoryViewModel>();
            builder.Services.AddTransient<ProductViewModel>();
            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
