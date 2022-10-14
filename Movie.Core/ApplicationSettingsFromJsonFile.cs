using EvilBaschdi.Settings;

namespace Movie.Core;

/// <inheritdoc cref="SettingsFromJsonFile" />
public class ApplicationSettingsFromJsonFile : WritableSettingsFromJsonFile, IApplicationSettingsFromJsonFile
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public ApplicationSettingsFromJsonFile()
        : base("Settings\\ApplicationSettings.json")
    {
    }
}