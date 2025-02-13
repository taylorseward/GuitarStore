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
            builder.Services.AddSingleton<GuitarViewModel>();
            builder.Services.AddTransient<GuitarPage>();
            builder.Services.AddTransient<AddGuitarPage>();
            builder.Services.AddSingleton<AddGuitarViewModel>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
