using Microsoft.Win32;

namespace Movie.Core
{
    /// <inheritdoc />
    public class XmlSettings : IXmlSettings
    {
        /// <inheritdoc />
        public void SaveToRegistry(string path, string dbType)
        {
            var softwareKey = Registry.CurrentUser.OpenSubKey("Software", true);

            var evilBaschdiKey = softwareKey?.CreateSubKey("EvilBaschdi",
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            var movieKey = evilBaschdiKey?.CreateSubKey("Movie",
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            using var settingsKey = movieKey?.CreateSubKey("Program Settings",
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (settingsKey == null)
            {
                return;
            }

            settingsKey.SetValue("XmlFilePath", path);
            settingsKey.SetValue("DbType", dbType);
        }

        /// <inheritdoc />
        public string FilePath
        {
            get
            {
                var evilBaschdiKey = Registry.CurrentUser.OpenSubKey(@"Software\EvilBaschdi",
                    RegistryKeyPermissionCheck.ReadSubTree);

                var movieKey = evilBaschdiKey?.OpenSubKey(@"Movie",
                    RegistryKeyPermissionCheck.ReadSubTree);

                if (movieKey == null)
                {
                    return "";
                }

                using var settingsKey = movieKey.OpenSubKey("Program Settings",
                    RegistryKeyPermissionCheck.ReadSubTree);
                return settingsKey?.GetValue("XmlFilePath", "").ToString() ?? "";
            }
        }

        /// <inheritdoc />
        public string DbType
        {
            get
            {
                var evilBaschdiKey = Registry.CurrentUser.OpenSubKey(@"Software\EvilBaschdi",
                    RegistryKeyPermissionCheck.ReadSubTree);

                var movieKey = evilBaschdiKey?.OpenSubKey(@"Movie",
                    RegistryKeyPermissionCheck.ReadSubTree);

                if (movieKey == null)
                {
                    return "";
                }

                using var settingsKey = movieKey.OpenSubKey("Program Settings",
                    RegistryKeyPermissionCheck.ReadSubTree);
                return settingsKey?.GetValue("DbType", "").ToString() ?? "movie";
            }
        }
    }
}