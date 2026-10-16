using Microsoft.Extensions.Logging;
using Pokedex.Data;
using Pokedex.Views;

namespace Pokedex
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<PokemonDatabase>();
            builder.Services.AddTransient<PokemonListaPage>();
            builder.Services.AddTransient<PokemonCadastroPage>();

            return builder.Build();
        }
    }
}
