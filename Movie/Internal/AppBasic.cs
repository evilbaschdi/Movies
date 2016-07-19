using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Movie.Core;

namespace Movie.Internal
{
    /// <summary>
    /// </summary>
    public class AppBasic : IAppBasic
    {
        private readonly MainWindow _mainWindow;
        private readonly ITools _tools;
        private readonly IXmlSettings _xmlSettings;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        /// <param name="mainWindow"></param>
        public AppBasic(MainWindow mainWindow)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException(nameof(mainWindow));
            }
            _mainWindow = mainWindow;
            _tools = new Tools();
            _xmlSettings = new XmlSettings();
        }

        private string StartupPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// </summary>
        public void Reset()
        {
            var fileDialog = new SaveFileDialog
                             {
                                 InitialDirectory = StartupPath(),
                                 FileName = "Movie.xml",
                                 Filter = "XML Document (*.xml)|*.xml",
                                 RestoreDirectory = true,
                                 CheckPathExists = true
                             };

            var result = fileDialog.ShowDialog();

            try
            {
                if (result == true)
                {
                    _mainWindow.DbPath.Text = fileDialog.FileName;
                    _xmlSettings.SaveToRegistry(_mainWindow.DbPath.Text, _mainWindow.DbType.Text);

                    using (var streamWriter = new StreamWriter(fileDialog.FileName))
                    {
                        streamWriter.WriteLine(_tools.GetResourceStreamText("newMovieDb.xml"));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error accessing resources!");
            }
        }

        /// <summary>
        /// </summary>
        public void Browse()
        {
            var fileDialog = new OpenFileDialog
                             {
                                 InitialDirectory = StartupPath(),
                                 Filter = "XML Document (*.xml)|*.xml",
                                 RestoreDirectory = true,
                                 CheckFileExists = true,
                                 CheckPathExists = true
                             };

            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                _mainWindow.DbPath.Text = fileDialog.FileName;
            }
        }

        /// <summary>
        /// </summary>
        public void Save()
        {
            _xmlSettings.SaveToRegistry(_mainWindow.DbPath.Text, _mainWindow.DbType.Text);
        }

        /// <summary>
        /// </summary>
        public void SetComboBoxItems()
        {
            switch (_xmlSettings.DbType)
            {
                case "movie":
                    _mainWindow.Format.ItemsSource = MovieFormats;
                    break;

                case "music":

                    _mainWindow.Format.ItemsSource = MusicFormats;
                    break;
            }
        }

        private IEnumerable<string> MusicFormats
        {
            get
            {
                var musicFormats = new ObservableCollection<string>
                                   {
                                       "CD",
                                       "MP3",
                                       "Kassette",
                                       "Schallplatte"
                                   };
                return musicFormats;
            }
        }

        private IEnumerable<string> MovieFormats
        {
            get
            {
                var movieFormats = new ObservableCollection<string>
                                   {
                                       "DVD",
                                       "Blu-ray",
                                       "VHS",
                                       "MP4",
                                       "MKV",
                                       "MPEG",
                                       "AVI",
                                       "ISO",
                                       "FLV",
                                       "OGG"
                                   };
                return movieFormats;
            }
        }
    }
}