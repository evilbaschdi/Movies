namespace Movie.AppCore
{
    /// <summary>
    ///     Interface for classes that provied AppConfig settings.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// </summary>
        string Accent { get; set; }

        /// <summary>
        /// </summary>
        string Theme { get; set; }

        /// <summary>
        /// </summary>
        string XmlFilePath { get; set; }

        /// <summary>
        /// </summary>
        string DbType { get; set; }
    }
}