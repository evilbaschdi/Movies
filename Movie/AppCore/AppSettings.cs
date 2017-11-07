using System.Configuration;
using System.Reflection;

namespace Movie.AppCore
{
    /// <summary>
    ///     Class that provides AppConfig settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        /// <summary>
        /// </summary>
        public string XmlFilePath
        {
            get => ConfigurationManager.AppSettings["XmlFilePath"];
            set => UpdateSetting("XmlFilePath", value);
        }

        /// <summary>
        /// </summary>
        public string DbType
        {
            get => ConfigurationManager.AppSettings["DbType"];
            set => UpdateSetting("DbType", value);
        }

        private static void UpdateSetting(string key, string value)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}