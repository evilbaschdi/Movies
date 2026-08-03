using Movie.AvaloniaUI.ViewModels;
using Movie.AvaloniaUI.Views;

namespace Movie.AvaloniaUI.DependencyInjection;

/// <summary />
public static class ConfigureWindowsAndViewModels
{
    /// <summary />
    public static void AddWindowsAndViewModels(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<MainWindowViewModel>();

        services.AddSingleton<AddEditMovieViewModel>();
        services.AddTransient<AddEditMovieDialog>();

        services.AddSingleton<LendViewModel>();
        services.AddTransient<LendDialog>();

        services.AddSingleton<WatchedViewModel>();
        services.AddTransient<WatchedDialog>();
    }
}