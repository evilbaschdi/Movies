using Microsoft.Win32;

namespace Movie.Core
{
    public class XmlSettings : IXmlSettings
    {
        public void SaveToRegistry(string path, string dbtype)
        {
            var softwareKey = Registry.CurrentUser.OpenSubKey("Software", true);
            if(softwareKey == null)
            {
                return;
            }

            var evilBaschdiKey = softwareKey.CreateSubKey("EvilBaschdi",
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            if(evilBaschdiKey == null)
            {
                return;
            }

            var movieKey = evilBaschdiKey.CreateSubKey("Movie",
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            if(movieKey == null)
            {
                return;
            }

            using(
                var settingsKey = movieKey.CreateSubKey("Program Settings",
                    RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                if(settingsKey == null)
                {
                    return;
                }
                settingsKey.SetValue("XmlFilePath", path);
                settingsKey.SetValue("DbType", dbtype);
            }
        }

        public string FilePath
        {
            get
            {
                var evilBaschdiKey = Registry.CurrentUser.OpenSubKey(@"Software\EvilBaschdi",
                    RegistryKeyPermissionCheck.ReadSubTree);

                if(evilBaschdiKey == null)
                {
                    return "";
                }

                var movieKey = evilBaschdiKey.OpenSubKey(@"Movie",
                    RegistryKeyPermissionCheck.ReadSubTree);

                if(movieKey == null)
                {
                    return "";
                }
                using(
                    var settingsKey = movieKey.OpenSubKey("Program Settings",
                        RegistryKeyPermissionCheck.ReadSubTree))
                {
                    return settingsKey != null ? settingsKey.GetValue("XmlFilePath", "").ToString() : "";
                }
            }
        }

        public string DbType
        {
            get
            {
                var evilBaschdiKey = Registry.CurrentUser.OpenSubKey(@"Software\EvilBaschdi",
                    RegistryKeyPermissionCheck.ReadSubTree);

                if(evilBaschdiKey == null)
                {
                    return "";
                }

                var movieKey = evilBaschdiKey.OpenSubKey(@"Movie",
                    RegistryKeyPermissionCheck.ReadSubTree);

                if(movieKey == null)
                {
                    return "";
                }
                using(
                    var settingsKey = movieKey.OpenSubKey("Program Settings",
                        RegistryKeyPermissionCheck.ReadSubTree))
                {
                    return settingsKey != null ? settingsKey.GetValue("DbType", "").ToString() : "movie";
                }
            }
        }

        //internal void CheckRegistrySettings()
        //{
        //    if (string.IsNullOrWhiteSpace(LoadFromRegistry()))
        //    {
        //        //ToDo: Dialog for choosing Path + Copy Default XML

        //        SaveToRegistry(@"D:\Users\Sebastian\Documents\Movie.xml");
        //    }

        //    XmlFilePath = LoadFromRegistry();
        //}
    }
}