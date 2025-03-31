using GuitarStore.Services;
using GuitarStore.ViewModels;
using GuitarStore.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;


namespace GuitarStore
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Database
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<AuthenticationService>();

            // Guitar
            builder.Services.AddTransient<GuitarViewModel>();
            builder.Services.AddTransient<GuitarPage>();
            builder.Services.AddTransient<AddGuitarPage>();
            builder.Services.AddTransient<AddGuitarViewModel>();
            builder.Services.AddTransient<EditGuitarPage>();

            // Amp
            builder.Services.AddTransient<AmpViewModel>();
            builder.Services.AddTransient<AmpPage>();
            builder.Services.AddTransient<AddAmpPage>();
            builder.Services.AddTransient<AddAmpViewModel>();

            // Pedals
            builder.Services.AddTransient<PedalViewModel>();
            builder.Services.AddTransient<PedalPage>();
            builder.Services.AddTransient<AddPedalPage>();
            builder.Services.AddTransient<AddPedalViewModel>();

            // Accessories
            builder.Services.AddTransient<AccessoryViewModel>();
            builder.Services.AddTransient<AccessoryPage>();
            builder.Services.AddTransient<AddAccessoryPage>();
            builder.Services.AddTransient<AddAccessoryViewModel>();

            // Inventory
            builder.Services.AddTransient<ProductViewModel>();
            builder.Services.AddTransient<InventoryPage>();

            // Home / Login / Register
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
