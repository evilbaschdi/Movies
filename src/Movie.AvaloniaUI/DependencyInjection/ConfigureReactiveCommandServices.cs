using Movie.AvaloniaUI.ViewModels.Internal;

namespace Movie.AvaloniaUI.DependencyInjection;

/// <summary />
public static class ConfigureReactiveCommandServices
{
    /// <summary />
    public static void AddReactiveCommandServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IInitReactiveCommands, InitReactiveCommands>();

        services.AddSingleton<IAboutWindowReactiveCommand, AboutWindowReactiveCommand>();
        services.AddSingleton<IAddMovieReactiveCommand, AddMovieReactiveCommand>();
        services.AddSingleton<IDeleteMovieReactiveCommand, DeleteMovieReactiveCommand>();
        services.AddSingleton<ILendReactiveCommand, LendReactiveCommand>();
        services.AddSingleton<IEditMovieReactiveCommand, EditMovieReactiveCommand>();
        services.AddSingleton<IGotBackReactiveCommand, GotBackReactiveCommand>();
        services.AddSingleton<ISettingsReactiveCommand, SettingsReactiveCommand>();
        services.AddSingleton<IWatchedReactiveCommand, WatchedReactiveCommand>();
    }
}