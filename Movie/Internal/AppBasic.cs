using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using JetBrains.Annotations;
using Microsoft.Win32;
using Movie.Core;

namespace Movie.Internal
{
    /// <summary>
    /// </summary>
    public class AppBasic : IAppBasic
    {
        private readonly MainWindow _mainWindow;
        private readonly IResourceStreamText _resourceStreamText;
        private readonly ISettings _settings;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="settings"></param>
        public AppBasic(MainWindow mainWindow, [NotNull] ISettings settings)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _resourceStreamText = new ResourceStreamText();
        }

        private static IEnumerable<string> MovieFormats
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

        private static IEnumerable<string> MusicFormats
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
                if (result != true)
                {
                    return;
                }

                _mainWindow.DbPath.Text = fileDialog.FileName;
                _settings.DbType = _mainWindow.DbType.Text;
                _settings.FilePath = _mainWindow.DbPath.Text;

                using var streamWriter = new StreamWriter(fileDialog.FileName);
                streamWriter.WriteLine(_resourceStreamText.ValueFor("newMovieDb.xml"));
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
            _settings.DbType = _mainWindow.DbType.Text;
            _settings.FilePath = _mainWindow.DbPath.Text;
        }

        /// <summary>
        /// </summary>
        public void SetComboBoxItems()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            // ReSharper disable once ConvertSwitchStatementToSwitchExpression
            switch (_settings.DbType)
            {
                case "movie":
                    _mainWindow.Format.ItemsSource = MovieFormats;
                    break;
                case "music":
                    _mainWindow.Format.ItemsSource = MusicFormats;
                    break;
            }
        }

        private static string StartupPath() => AppDomain.CurrentDomain.BaseDirectory;
    }
}