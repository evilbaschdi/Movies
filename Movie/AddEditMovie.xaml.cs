using System;
using System.Windows;
using MahApps.Metro.Controls;
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
        private MovieRecord _movie;
        private readonly MainWindow _mainWindow;
        public static string CurrentId;

        public AddEditMovie()
        {
            InitializeComponent();
            _mainWindow = new MainWindow();

            if(string.IsNullOrWhiteSpace(CurrentId))
            {
                Title = "Add Movie";
                _mode = "add";
            }
            else
            {
                Title = "Edit Movie";
                _mode = "edit";
                LoadData();
            }
        }

        private void LoadData()
        {
            _movie = List.GetMovieById(CurrentId);
            if(_movie != null)
            {
                Name.Text = _movie.Name;
                Year.Text = _movie.Year;
                Format.Text = _movie.Format;
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

        private void SaveAndAddNew(bool addNew)
        {
            if(IsDuplicate())
            {
                MessageBox.Show("This movie already exists in the database");
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
            Year.Text = "";
            Format.Text = "";
        }

        public void SaveOrUpdateData()
        {
            if(IsValidate())
            {
                InsertOrUpdate();
                InsertOrUpdateAction();
            }
        }

        public void InsertOrUpdateAction()
        {
            _movie = new MovieRecord
            {
                Id = CurrentId,
                Name = Name.Text,
                Year = Year.Text,
                Format = Format.Text,
                Distributed = "False"
            };

            try
            {
                switch(_action)
                {
                    case "insert":
                        List.Insert(_movie);
                        break;

                    case "update":
                        List.Update(_movie);
                        break;
                }
            }
            catch(Exception exception)
            {
                _exception = string.Format("'{0}' failed {1} to database\n Message : {2}", Name.Text, _action,
                    exception.Message);
                MessageBox.Show(_exception, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _mainWindow.Populate();
        }

        public void NewEntry()
        {
            ClearForm();
            _movie = null;
            Name.Focus();
        }

        private void InsertOrUpdate()
        {
            _movie = List.GetMovieById(CurrentId);
            _action = _movie != null ? "update" : "insert";
        }

        private bool IsDuplicate()
        {
            return List.GetMovieByName(Name.Text) != null && _mode == "add";
        }

        private bool IsValidate()
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