using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        private string _currentId;
        private string _exception;
        private IXmlSettings _xmlSettings;
        private IMovieRecord _movieRecord;
        private IMovies _movies;

        /// <summary>
        ///     MainWindows.
        ///     Gets a new movie list instance.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ValidateSettings();
        }

        #region DataGrid Logic

        /// <summary>
        ///     (Re)Loads movie datasource to grid ans calls sorting.
        /// </summary>
        public void Populate()
        {
            //var listCollectionView = new ListCollectionView(_movies.MovieDataView());
            //if (listCollectionView.GroupDescriptions != null)
            //{
            //    listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Distributed"));
            //    listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Name"));
            //}
            //MovieGrid.ItemsSource = listCollectionView;
            MovieGrid.ItemsSource = _movies.MovieDataView();
            Sorting();
        }

        private void Sorting()
        {
            //create a collection view for the datasoruce binded with grid

            var dataView = CollectionViewSource.GetDefaultView(MovieGrid.ItemsSource);
            //clear the existing sort order
            dataView.SortDescriptions.Clear();
            //create a new sort order for the sorting that is done lastly
            dataView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //refresh the view which in turn refresh the grid
            dataView.Refresh();
        }

        private void LoadData(string id)
        {
            _movieRecord = _movies.GetMovieById(id);
            if(_movieRecord != null)
            {
                _currentId = _movieRecord.Id;
            }
        }

        private void MovieGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MovieGrid.SelectedItem == null)
            {
                return;
            }
            var dataRowView = (DataRowView) MovieGrid.SelectedItem;

            var id = dataRowView.Row["Id"].ToString();
            var distributed = dataRowView.Row["Distributed"].ToString();

            LoadData(id);

            DistributeMenuItem.IsEnabled = distributed != "True";
            GotBackMenuItem.IsEnabled = distributed == "True";
        }

        #endregion DataGrid Logic

        #region Settings

        private void ValidateSettings()
        {
            _xmlSettings = new XmlSettings();

            Title = _xmlSettings.DbType == "music" ? "Music" : "Movies";
            New.Content = "Add new " + _xmlSettings.DbType;

            if(!string.IsNullOrWhiteSpace(_xmlSettings.FilePath))
            {
                SearchCategory.IsEnabled = true;
                SearchFilter.IsEnabled = true;
                New.IsEnabled = true;
                _movies = new Movies();
                SearchCategory.Text = "Name";
                Populate();
            }
            else
            {
                new Settings().Show();
                SearchCategory.IsEnabled = false;
                SearchFilter.IsEnabled = false;
                New.IsEnabled = false;
            }
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            new Settings().Show();
        }

        #endregion Settings

        #region Add Edit Movie

        private void NewClick(object sender, RoutedEventArgs e)
        {
            AddEditMovie.CurrentId = null;
            new AddEditMovie().Show();
        }

        private void Edit()
        {
            AddEditMovie.CurrentId = _currentId;
            new AddEditMovie().Show();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void MovieGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        #endregion Add Edit Movie

        #region Delete Movie

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteData();
        }

        private async void DeleteData()
        {
            var options = new MetroDialogSettings
            {
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MetroDialogOptions = options;
            try

            {
                var delete =
                    await
                        this.ShowMessageAsync("You are about to delete",
                            string.Format("'{0}'", _movieRecord.Name),
                            MessageDialogStyle.AffirmativeAndNegative, options);

                if(delete == MessageDialogResult.Affirmative)
                {
                    _movies.Delete(_currentId);
                }
            }
            catch(Exception exp)
            {
                _exception = string.Format("Record {0} failed delete to database\n Message : {1}",
                    _movieRecord.Name.Trim(), exp.Message);

                MessageBox.Show(_exception, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            Populate();
        }

        #endregion Delete Movie

        #region Distribution

        private void DistributeClick(object sender, RoutedEventArgs e)
        {
            _movieRecord.Distributed = "True";
            _movies.Update(_movieRecord);
            MovieGrid.SelectedItem = null;
        }

        private void GotBackClick(object sender, RoutedEventArgs e)
        {
            _movieRecord.Distributed = "False";
            _movies.Update(_movieRecord);
            MovieGrid.SelectedItem = null;
        }

        #endregion Distribution

        #region Search

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Populate(SearchFilter.Text, SearchCategory.Text);
        }

        private void Populate(string searchText, string searchCategory)
        {
            MovieGrid.ItemsSource = _movies.MovieDataView(searchText, searchCategory);
            Sorting();
        }

        private void SearchCategoryOnDropDownClosed(object sender, EventArgs e)
        {
            if(SearchCategory.Text == "Distributed")
            {
                SearchFilter.KeyDown += SearchFilterKeyPress;
                SearchFilter.MaxLength = 1;
            }
        }

        private void SearchFilterKeyPress(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.F && e.Key != Key.T)
            {
                e.Handled = true;
            }
        }

        #endregion Search
    }
}