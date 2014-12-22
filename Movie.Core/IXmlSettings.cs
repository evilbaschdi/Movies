namespace Movie.Core
{
    public interface IXmlSettings
    {
        string FilePath { get; }

        void SaveToRegistry(string path);
    }
}