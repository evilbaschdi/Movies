using EvilBaschdi.Settings;
using EvilBaschdi.Settings.Writable;

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