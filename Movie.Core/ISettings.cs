namespace Movie.Core;

/// <summary>
/// </summary>
public interface ISettings
{
    /// <summary>
    /// </summary>
    string DbType { get; set; }

    /// <summary>
    /// </summary>
    string FilePath { get; set; }
}