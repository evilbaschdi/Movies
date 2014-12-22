using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using Movie.Core;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        private IXmlSettings _xmlSettings;
        private readonly ITools _tools;
        private readonly MainWindow _mainWindow;

        /// <summary>
        ///     InitializeComponent
        /// </summary>
        public Settings()
        {
            InitializeComponent();
            //_mainWindow = new MainWindow();
            _tools = new Tools();
            LoadXmlSettings();
        }

        private void LoadXmlSettings()
        {
            _xmlSettings = new XmlSettings();
            if(!string.IsNullOrWhiteSpace(_xmlSettings.FilePath))
            {
                Path.Text = _xmlSettings.FilePath;
                if(!File.Exists(Path.Text))
                {
                    Path.Background = Brushes.Maroon;
                }
            }
        }

        private void CancelClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CleanupAndClose();
        }

        private void CleanupAndClose()
        {
            //_mainWindow.Close();
            Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            _xmlSettings.SaveToRegistry(Path.Text);
            //_mainWindow.Populate();
        }

        private string StartupPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
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

            if(result == true)
            {
                Path.Text = fileDialog.FileName;
            }
        }

        private void ResetClick(object sender, RoutedEventArgs e)
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
                if(result == true)
                {
                    Path.Text = fileDialog.FileName;
                    _xmlSettings.SaveToRegistry(Path.Text);

                    using(var streamWriter = new StreamWriter(fileDialog.FileName))
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
    }
}