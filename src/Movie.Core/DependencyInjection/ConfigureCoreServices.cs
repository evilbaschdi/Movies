using Movie.Core.Models;

namespace Movie.Core.DependencyInjection;

/// <summary />
public static class ConfigureCoreServices
{
    /// <summary />
    public static void AddCoreServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IApplicationSettingsFromJsonFile, ApplicationSettingsFromJsonFile>();
        services.AddSingleton<ICurrentMovie, CurrentMovie>();
        services.AddSingleton<IJsonDatabase, JsonDatabase>();
        services.AddSingleton<IMovies, Movies>();
        services.AddSingleton<IResourceStreamText, ResourceStreamText>();
        services.AddSingleton<ISettings, Settings>();
    }
}