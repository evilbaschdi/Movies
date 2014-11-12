using System;
using Microsoft.Win32;

namespace Movie.Core
{
    public class SettingsStore
    {
        internal string XmlFilePath { get; private set; }

        internal string StartupPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private string LoadFromRegistry()
        {
            var movieKey = Registry.CurrentUser.OpenSubKey(@"Software\EvilBaschdi\Movie",
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

        private void SaveToRegistry(string path)
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
            }
        }

        internal void CheckRegistrySettings()
        {
            if(string.IsNullOrWhiteSpace(LoadFromRegistry()))
            {
                //ToDo: Dialog for choosing Path + Copy Default XML

                SaveToRegistry(@"D:\Users\Sebastian\Documents\Movie.xml");
            }

            XmlFilePath = LoadFromRegistry();
        }
    }
}