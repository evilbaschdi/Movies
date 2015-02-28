using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;
using Movie.Internal;

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
        private string _dbType;
        private readonly IXmlSettings _xmlSettings;
        private IMovieRecord _movieRecord;
        private readonly IMovies _movies;

        private readonly ApplicationStyle _style;
        private readonly ApplicationSettings _settings;
        private readonly AddEdit _addEdit;

        /// <summary>
        ///     MainWindows.
        ///     Gets a new movie list instance.
        /// </summary>
        public MainWindow()
        {
            _style = new ApplicationStyle(this);
            _settings = new ApplicationSettings(this);
            _addEdit = new AddEdit(this);
            _xmlSettings = new XmlSettings();
            _movies = new Movies();
            InitializeComponent();
            _style.Load();
            ValidateSettings();
            _settings.SetComboBoxItems();
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
            Year.Maximum = DateTime.Now.Year;
            _dbType = _xmlSettings.DbType == "music" ? "music" : "movie";
            DbType.Text = !string.IsNullOrWhiteSpace(_xmlSettings.DbType) ? _xmlSettings.DbType : "movie";
            Title = _dbType;
            NewContent.Text = string.Format("add new {0}", _dbType);

            if(!string.IsNullOrWhiteSpace(_xmlSettings.FilePath))
            {
                SearchCategory.IsEnabled = true;
                SearchFilter.IsEnabled = true;
                New.IsEnabled = true;
                SearchCategory.Text = "Name";
                DbPath.Text = _xmlSettings.FilePath;
                if(!File.Exists(DbPath.Text))
                {
                    DbPath.Background = Brushes.Maroon;
                }
                Populate();
            }
            else
            {
                SearchCategory.IsEnabled = false;
                SearchFilter.IsEnabled = false;
                New.IsEnabled = false;
            }
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _settings.Save();
            Populate();
        }

        private void CancelSettingsClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            _settings.Browse();
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            _settings.Reset();
        }

        private void ToggleFlyout(int index)
        {
            var flyout = (Flyout) Flyouts.Items[index];

            if(flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }

        #endregion Settings

        #region Add Edit Movie

        private void NewClick(object sender, RoutedEventArgs e)
        {
            _addEdit.CurrentId = null;
            _addEdit.Mode = "add";
            AddEditFlyout.Header = string.Format("add new {0}", _dbType);
            Year.Value = Year.Maximum;
            _currentId = string.Empty;
            ToggleFlyout(1);
        }

        private void Edit()
        {
            _addEdit.CurrentId = _currentId;
            _addEdit.Mode = "edit";
            AddEditFlyout.Header = string.Format("edit {0}", _dbType);
            LoadCurrentMovieData();
            ToggleFlyout(1);
        }

        private void LoadCurrentMovieData()
        {
            _movieRecord = _movies.GetMovieById(_currentId);
            if(_movieRecord != null)
            {
                Name.Text = _movieRecord.Name;
                Year.Value = string.IsNullOrWhiteSpace(_movieRecord.Year)
                    ? Year.Maximum
                    : Convert.ToDouble(_movieRecord.Year);
                Format.Text = _movieRecord.Format;
            }
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void MovieGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        private void SaveClick(object sender, RoutedEventArgs routedEventArgs)
        {
            _addEdit.MovieData(Name.Text, Year.Value, Format.Text);
            _addEdit.SaveAndAddNew(false);
        }

        private void SaveAndNewClick(object sender, RoutedEventArgs e)
        {
            _addEdit.MovieData(Name.Text, Year.Value, Format.Text);
            _addEdit.SaveAndAddNew(true);
        }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public async void ShowErrorMessage(string message)
        {
            var options = new MetroDialogSettings
            {
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MetroDialogOptions = options;
            await this.ShowMessageAsync("Error", message);
        }

        /// <summary>
        /// </summary>
        public void NewEntry()
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _movieRecord = null;
            Name.Focus();
            Name.Text = "";
            Year.Value = Year.Maximum;
            Format.Text = "";
        }

        private void CancelClick(object sender, RoutedEventArgs routedEventArgs)
        {
            CleanupAndClose();
        }

        /// <summary>
        /// </summary>
        public void CleanupAndClose()
        {
            Name.Clear();
            _currentId = null;
            ToggleFlyout(1);
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

        #region Style

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _style.SetAccent(sender, e);
        }

        #endregion Style
    }
}