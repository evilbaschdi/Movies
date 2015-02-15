﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for AddEditMovie.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class AddEditMovie : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        private string _action;
        private readonly string _mode;
        private string _exception;
        private IMovieRecord _movieRecord;
        private readonly IXmlSettings _xmlSettings;
        private readonly IMovies _movies;
        private readonly MainWindow _mainWindow;

        /// <summary>
        ///     Id of the currently selected movie.
        ///     Is emtpy, if we are not in "edit" mode and does an "add".
        /// </summary>
        public static string CurrentId;

        /// <summary>
        ///     Initilalizes the MetroWindows and decides betweed modes "add" and "edit".
        /// </summary>
        public AddEditMovie()
        {
            InitializeComponent();
            _mainWindow = new MainWindow();
            _xmlSettings = new XmlSettings();
            _movies = new Movies();
            Year.Maximum = DateTime.Now.Year;
            SetComboBoxItems();

            var dbType = _xmlSettings.DbType == "music" ? "Music" : "Movie";

            if(string.IsNullOrWhiteSpace(CurrentId))
            {
                Title = string.Format("Add {0}", dbType);
                _mode = "add";
                Year.Value = Year.Maximum;
            }
            else
            {
                Title = string.Format("Edit {0}", dbType);
                _mode = "edit";
                LoadData();
            }
        }

        private void SetComboBoxItems()
        {
            switch(_xmlSettings.DbType)
            {
                case "movie":
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
                    Format.ItemsSource = movieFormats;
                    break;

                case "music":
                    var musicFormats = new ObservableCollection<string>
                    {
                        "CD",
                        "MP3",
                        "Kassette",
                        "Schallplatte",
                    };
                    Format.ItemsSource = musicFormats;
                    break;
            }
        }

        private void LoadData()
        {
            _movieRecord = _movies.GetMovieById(CurrentId);
            if(_movieRecord != null)
            {
                Name.Text = _movieRecord.Name;
                Year.Value = string.IsNullOrWhiteSpace(_movieRecord.Year)
                    ? Year.Maximum
                    : Convert.ToDouble(_movieRecord.Year);
                Format.Text = _movieRecord.Format;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveAndAddNew(false);
        }

        private void SaveAndNewClick(object sender, RoutedEventArgs e)
        {
            SaveAndAddNew(true);
        }

        private async void SaveAndAddNew(bool addNew)
        {
            if(IsDuplicate())
            {
                var options = new MetroDialogSettings
                {
                    ColorScheme = MetroDialogColorScheme.Theme
                };

                MetroDialogOptions = options;
                await this.ShowMessageAsync("Already existing!",
                    string.Format("'{0}'", Name.Text));
            }
            else
            {
                SaveOrUpdateData();

                if(addNew)
                {
                    NewEntry();
                }
                else
                {
                    CleanupAndClose();
                }
            }
        }

        private void CancelClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CleanupAndClose();
        }

        private void ClearForm()
        {
            Name.Text = "";
            Year.Value = Year.Maximum;
            Format.Text = "";
        }

        private void SaveOrUpdateData()
        {
            if(IsValid())
            {
                InsertOrUpdate();
                InsertOrUpdateAction();
            }
        }

        private void InsertOrUpdateAction()
        {
            _movieRecord = new MovieRecord
            {
                Id = CurrentId,
                Name = Name.Text,
                Year = Year.Value.ToString(),
                Format = Format.Text,
                Distributed = "False"
            };

            try
            {
                switch(_action)
                {
                    case "insert":
                        _movies.Insert(_movieRecord);
                        break;

                    case "update":
                        _movies.Update(_movieRecord);
                        break;
                }
            }
            catch(Exception exception)
            {
                _exception = string.Format("'{0}' failed {1} to database\n Message : {2}", Name.Text, _action,
                    exception.Message);
                ShowErrorMessage(_exception);
            }

            _mainWindow.Populate();
        }

        private async void ShowErrorMessage(string message)
        {
            var options = new MetroDialogSettings
            {
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MetroDialogOptions = options;
            await this.ShowMessageAsync("Error", message);
        }

        private void NewEntry()
        {
            ClearForm();
            _movieRecord = null;
            Name.Focus();
        }

        private void InsertOrUpdate()
        {
            _movieRecord = _movies.GetMovieById(CurrentId);
            _action = _movieRecord != null ? "update" : "insert";
        }

        private bool IsDuplicate()
        {
            return _movies.GetMovieByName(Name.Text) != null && _mode == "add";
        }

        private bool IsValid()
        {
            return Name.Text != string.Empty;
        }

        private void CleanupAndClose()
        {
            _mainWindow.Close();
            Close();
        }

        //private void NameOnLostFocus(object sender, RoutedEventArgs e)
        //{
        //    var searchContainer = _tmDb.GetMovieByTitle(Name.Text);

        //    if(searchContainer.Results.Count == 1)
        //    {
        //        var movie = searchContainer.Results.FirstOrDefault();
        //        if(movie != null)
        //        {
        //            Year.Text = movie.ReleaseDate != null ? movie.ReleaseDate.Value.Year.ToString("D") : "";
        //        }
        //    }
        //    else
        //    {
        //        var seachCollection = _tmDb.GetMovieSearchCollection(Name.Text);

        //        ChooseMatchingMoviePart.TmDb = _tmDb;
        //        ChooseMatchingMoviePart.MovieParts = seachCollection.Parts;
        //        new ChooseMatchingMoviePart().Show();
        //    }
        //}
    }
}