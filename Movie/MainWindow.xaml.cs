using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Movie.AppCore;
using Movie.Core;
using Movie.Internal;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        private IMovieRecord _movieRecord;

        private readonly IXmlSettings _xmlSettings;
        private readonly ISettings _coreSettings;
        private readonly IMetroStyle _style;
        private readonly IMovies _movies;
        private readonly IAppBasic _appBasic;
        private readonly IAddEdit _addEdit;
        private int _overrideProtection;
        private string _currentId;
        private string _exception;
        private string _dbType;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable

        /// <summary>
        ///     MainWindows.
        ///     Gets a new movie list instance.
        /// </summary>
        public MainWindow()
        {
            _appBasic = new AppBasic(this);
            _addEdit = new AddEdit(this);
            _xmlSettings = new XmlSettings();
            _movies = new Movies();
            _coreSettings = new CoreSettings();
            InitializeComponent();
            _style = new MetroStyle(this, Accent, Dark, Light, _coreSettings);
            _style.Load(true, true);
            ValidateSettings();
            _appBasic.SetComboBoxItems();
        }

        #region DataGrid Logic

        /// <summary>
        ///     (Re)Loads movie datasource to grid ans calls sorting.
        /// </summary>
        public void Populate()
        {
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
            NewContent.Text = $"add new {_dbType}";

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

            _overrideProtection = 1;
        }

        private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _appBasic.Save();
            Populate();
        }

        private void CancelSettingsClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            _appBasic.Browse();
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            _appBasic.Reset();
        }

        private void ToggleFlyout(int index, bool stayOpen = false)
        {
            var activeFlyout = (Flyout) Flyouts.Items[index];
            if(activeFlyout == null)
            {
                return;
            }

            //Parallel.ForEach(Flyouts.Items.Cast<Flyout>().Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name),
            //    nonactiveFlyout => { nonactiveFlyout.IsOpen = false; }
            //    );

            foreach(
                var nonactiveFlyout in
                    Flyouts.Items.Cast<Flyout>()
                        .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if(activeFlyout.IsOpen && stayOpen)
            {
                activeFlyout.IsOpen = true;
            }
            else
            {
                activeFlyout.IsOpen = !activeFlyout.IsOpen;
            }
        }

        #endregion Settings

        #region Add Edit Movie

        private void NewClick(object sender, RoutedEventArgs e)
        {
            _addEdit.CurrentId = null;
            _addEdit.Mode = "add";
            AddEditFlyout.Header = $"add new {_dbType}";
            Year.Value = Year.Maximum;
            _currentId = string.Empty;
            ToggleFlyout(1);
        }

        private void Edit()
        {
            _addEdit.CurrentId = _currentId;
            _addEdit.Mode = "edit";
            AddEditFlyout.Header = $"edit {_dbType}";
            LoadCurrentMovieData();
            ToggleFlyout(1);
        }

        private void LoadCurrentMovieData()
        {
            _movieRecord = _movies.GetMovieById(_currentId);
            if(_movieRecord != null)
            {
                MovieName.Text = _movieRecord.Name;
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
            _addEdit.MovieData(MovieName.Text, Year.Value, Format.Text);
            _addEdit.SaveAndAddNew(false);
        }

        private void SaveAndNewClick(object sender, RoutedEventArgs e)
        {
            _addEdit.MovieData(MovieName.Text, Year.Value, Format.Text);
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
            MovieName.Focus();
            MovieName.Text = "";
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
            MovieName.Clear();
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
                            $"'{_movieRecord.Name}'",
                            MessageDialogStyle.AffirmativeAndNegative, options);

                if(delete == MessageDialogResult.Affirmative)
                {
                    _movies.Delete(_currentId);
                }
            }
            catch(Exception exp)
            {
                _exception =
                    $"failed to delete record {_movieRecord.Name.Trim()} from database\n Message : {exp.Message}";

                MessageBox.Show(_exception, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            Populate();
        }

        #endregion Delete Movie

        #region Distribution

        private void DistributeClick(object sender, RoutedEventArgs e)
        {
            DistributedFlyout.Header = $"distribute{Environment.NewLine}'{_movieRecord.Name}'";
            ToggleFlyout(3, true);
        }

        private void DistributeCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var result = ((CheckBox) sender).IsChecked;

            if(result.HasValue)
            {
                if(result.Value)
                {
                    DistributeClick(sender, e);
                }
                else
                {
                    GotBackClick(sender, e);
                }
            }
        }

        private void SaveDistributedToClick(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(DistributedTo.Text))
            {
                _movieRecord.Distributed = "True";
                _movieRecord.DistributedTo = DistributedTo.Text;
                _movies.Update(_movieRecord);
                MovieGrid.SelectedItem = null;
            }
        }

        private void GotBackClick(object sender, RoutedEventArgs e)
        {
            _movieRecord.Distributed = "False";
            _movieRecord.DistributedTo = "";
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

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle

        #region Watched movie

        private void SaveWatchDateClick(object sender, RoutedEventArgs e)
        {
            if(LastTimeWatched.SelectedDate.HasValue)
            {
                _movieRecord.Watched = LastTimeWatched.SelectedDate.Value.ToShortDateString();

                _movies.Update(_movieRecord);
                MovieGrid.SelectedItem = null;
            }
        }

        private void ToggleWatchedFlyoutClick(object sender, RoutedEventArgs e)
        {
            LastTimeWatched.SelectedDate = !string.IsNullOrWhiteSpace(_movieRecord.Watched)
                ? DateTime.Parse(_movieRecord.Watched)
                : DateTime.Now;

            WatchedFlyout.Header = $"last time of watching{Environment.NewLine}'{_movieRecord.Name}'";
            ToggleFlyout(2, true);
        }

        private void CommandBindingCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBindingExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((Calendar) e.Parameter).SelectedDate = DateTime.Now.Date;
        }

        #endregion Watched movie
    }
}