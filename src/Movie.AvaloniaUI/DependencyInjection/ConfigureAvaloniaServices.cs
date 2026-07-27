using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Movie.AvaloniaUI.DependencyInjection;

/// <summary />
public static class ConfigureAvaloniaServices
{
    /// <summary />
    public static void AddAvaloniaServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IMainWindowByApplicationLifetime, MainWindowByApplicationLifetime>();
    }
}