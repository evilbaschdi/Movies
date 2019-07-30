namespace Movie.Core
{
    /// <summary>
    /// </summary>
    public interface IXmlSettings
    {
        /// <summary>
        /// </summary>
        string DbType { get; }

        /// <summary>
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dbType"></param>
        void SaveToRegistry(string path, string dbType);
    }
}