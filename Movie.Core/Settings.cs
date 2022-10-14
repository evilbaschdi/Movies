using JetBrains.Annotations;

namespace Movie.Core;

/// <inheritdoc />
public class Settings : ISettings
{
    private readonly IApplicationSettingsFromJsonFile _applicationSettingsFromJsonFile;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="applicationSettingsFromJsonFile"></param>
    public Settings([NotNull] IApplicationSettingsFromJsonFile applicationSettingsFromJsonFile)
    {
        _applicationSettingsFromJsonFile = applicationSettingsFromJsonFile ?? throw new ArgumentNullException(nameof(applicationSettingsFromJsonFile));
    }

    /// <inheritdoc />
    public string DbType
    {
        get => _applicationSettingsFromJsonFile.Value["DbType"];
        set => _applicationSettingsFromJsonFile.Value["DbType"] = value;
    }

    /// <inheritdoc />
    public string FilePath
    {
        get => _applicationSettingsFromJsonFile.Value["FilePath"];
        set => _applicationSettingsFromJsonFile.Value["FilePath"] = value;
    }
}