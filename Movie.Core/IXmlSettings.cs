namespace Movie.Core
{
    public interface IXmlSettings
    {
        string FilePath { get; }

        string DbType { get; }

        void SaveToRegistry(string path, string dbtype);
    }
}