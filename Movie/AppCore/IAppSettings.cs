namespace Movie.AppCore
{
    /// <summary>
    ///     Interface for classes that provied AppConfig settings.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// </summary>
        string XmlFilePath { get; set; }

        /// <summary>
        /// </summary>
        string DbType { get; set; }
    }
}