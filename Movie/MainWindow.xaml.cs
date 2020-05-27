using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Controls.About;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;
using Movie.Core.Models;
using Movie.Internal;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IAddEdit _addEdit;
        private readonly IAppBasic _appBasic;
        private readonly IMovies _movies;
        private readonly IXmlSettings _xmlSettings;
        private string _currentId;
        private IMovieRecord _currentMovieRecord;
        private string _dbType;
        private string _exception;
        private ListCollectionView _listCollectionView;
        private string _prevSortHeader;
        private SortDescription _sd = new SortDescription("Name", ListSortDirection.Ascending);
        private string _sortHeader;

        /// <summary>
        ///     MainWindows.
        ///     Gets a new movie list instance.
        /// </summary>
        public MainWindow()
        {
            _appBasic = new AppBasic(this);

            _xmlSettings = new XmlSettings();
            IXmlDatabase xmlDatabase = new XmlDatabase(_xmlSettings);
            ITransformDataRowToMovieRecord transformDataRowToMovieRecord = new TransformDataRowToMovieRecord();
            _movies = new Movies(xmlDatabase, transformDataRowToMovieRecord);

            InitializeComponent();

            IApplicationStyle style = new ApplicationStyle();
            style.Load(true, true);
            _addEdit = new AddEdit(this, _movies);
            ValidateSettings();
            _appBasic.SetComboBoxItems();
        }

        private void AboutWindowClick(object sender, RoutedEventArgs e)
        {
            var assembly = typeof(MainWindow).Assembly;
            IAboutContent aboutWindowContent = new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\movie_512.png");

            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent)
                              };

            aboutWindow.ShowDialog();
        }


        #region DataGrid Logic

        /// <summary>
        ///     (Re)Loads movie data source to grid ans calls sorting.
        /// </summary>
        public void Populate()
        {
            _listCollectionView = new ListCollectionView(_movies.Value);
            _listCollectionView.SortDescriptions.Add(_sd);
            MovieGrid.ItemsSource = _listCollectionView;
        }

        private void DataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            _sortHeader = e.Column.SortMemberPath;

            _sd = _sortHeader == _prevSortHeader
                ? new SortDescription(_sortHeader, ListSortDirection.Descending)
                : new SortDescription(_sortHeader, ListSortDirection.Ascending);
            _prevSortHeader = _sortHeader;
        }

        private void MovieGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieGrid.SelectedItem == null)
            {
                return;
            }

            _currentMovieRecord = (MovieRecord) MovieGrid.SelectedItem;

            _currentId = _currentMovieRecord.Id;
            var distributed = _currentMovieRecord.Distributed;
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

            if (!string.IsNullOrWhiteSpace(_xmlSettings.FilePath))
            {
                SearchCategory.IsEnabled = true;
                SearchFilter.IsEnabled = true;
                New.IsEnabled = true;
                SearchCategory.Text = "Name";
                DbPath.Text = _xmlSettings.FilePath;
                if (!File.Exists(DbPath.Text))
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

        private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _appBasic.Save();
            Populate();
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
            if (activeFlyout == null)
            {
                return;
            }

            foreach (
                var nonactiveFlyout in
                Flyouts.Items.Cast<Flyout>()
                       .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if (activeFlyout.IsOpen && stayOpen)
            {
                activeFlyout.IsOpen = true;
            }
            else
            {
                activeFlyout.IsOpen = !activeFlyout.IsOpen;
            }

            activeFlyout.ClosingFinished += ActiveFlyoutClosingFinished;
        }

        private void ActiveFlyoutClosingFinished(object sender, RoutedEventArgs e)
        {
            Populate();
        }

        #endregion Settings

        #region Add Edit Movie

        private void NewClick(object sender, RoutedEventArgs e)
        {
            _currentId = null;
            _addEdit.Mode = "add";
            AddEditFlyout.Header = $"add new {_dbType}";
            Year.Value = Year.Maximum;
            _currentId = string.Empty;
            ToggleFlyout(1);
        }

        private void Edit()
        {
            _addEdit.Mode = "edit";
            AddEditFlyout.Header = $"edit {_dbType}";
            LoadCurrentMovieData();
            ToggleFlyout(1);
        }

        private void LoadCurrentMovieData()
        {
            _currentMovieRecord = _movies.ValueById(_currentId);
            if (_currentMovieRecord == null)
            {
                return;
            }

            MovieName.Text = _currentMovieRecord.Name;
            Year.Value = string.IsNullOrWhiteSpace(_currentMovieRecord.Year)
                ? Year.Maximum
                : Convert.ToDouble(_currentMovieRecord.Year);
            Format.Text = _currentMovieRecord.Format;
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
            _addEdit.MovieData(MovieName.Text, Year.Value, Format.Text, _currentId);
            _addEdit.Save();
            CleanupAndClose();
        }

        private void SaveAndNewClick(object sender, RoutedEventArgs e)
        {
            _addEdit.MovieData(MovieName.Text, Year.Value, Format.Text, _currentId);
            _addEdit.Save();
            NewEntry();
        }

        /// <summary>
        /// </summary>
        public void NewEntry()
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _currentMovieRecord = null;
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
            try

            {
                var delete =
                    await
                        this.ShowMessageAsync("Delete", $"You are about to delete '{_currentMovieRecord.Name}'", MessageDialogStyle.AffirmativeAndNegative);

                if (delete == MessageDialogResult.Affirmative)
                {
                    _movies.Delete(_currentId);
                }
            }
            catch (Exception exp)
            {
                _exception =
                    $"failed to delete record {_currentMovieRecord.Name.Trim()} from database\n Message : {exp.Message}";
                await this.ShowMessageAsync("Delete failed", _exception);
            }

            Populate();
        }

        #endregion Delete Movie

        #region Distribution

        private void DistributeClick(object sender, RoutedEventArgs e)
        {
            DistributedFlyout.Header = $"distribute{Environment.NewLine}'{_currentMovieRecord.Name}'";
            ToggleFlyout(3, true);
        }

        private void DistributeCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var result = ((CheckBox) sender).IsChecked;

            if (!result.HasValue)
            {
                return;
            }

            if (result.Value)
            {
                DistributeClick(sender, e);
            }
            else
            {
                GotBackClick(sender, e);
            }
        }

        private void SaveDistributedToClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DistributedTo.Text))
            {
                return;
            }

            _currentMovieRecord.Distributed = "True";
            _currentMovieRecord.DistributedTo = DistributedTo.Text;
            _movies.Update(_currentMovieRecord);
            MovieGrid.SelectedItem = null;
        }

        private void GotBackClick(object sender, RoutedEventArgs e)
        {
            _currentMovieRecord.Distributed = "False";
            _currentMovieRecord.DistributedTo = "";
            _movies.Update(_currentMovieRecord);
            MovieGrid.SelectedItem = null;
        }

        #endregion Distribution

        #region Search

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            _listCollectionView.Filter = m => Filter(m, SearchCategory.Text, SearchFilter.Text);
        }

        private bool Filter(object m, string category, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            var movieRecord = (MovieRecord) m;

            return category switch
            {
                "Name" => movieRecord.Name.Contains(text, StringComparison.InvariantCultureIgnoreCase),
                "Year" => movieRecord.Year.Contains(text, StringComparison.InvariantCultureIgnoreCase),
                "Format" => movieRecord.Format.Contains(text, StringComparison.InvariantCultureIgnoreCase),
                "Distributed" => movieRecord.Distributed.Contains(text, StringComparison.InvariantCultureIgnoreCase),
                _ => true
            };
        }

        private void SearchCategoryOnDropDownClosed(object sender, EventArgs e)
        {
            if (!string.Equals(SearchCategory.Text, "Distributed", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            SearchFilter.KeyDown += SearchFilterKeyPress;
            SearchFilter.MaxLength = 1;
        }

        private static void SearchFilterKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F && e.Key != Key.T)
            {
                e.Handled = true;
            }
        }

        #endregion Search

        #region Watched movie

        private void SaveWatchDateClick(object sender, RoutedEventArgs e)
        {
            if (!LastTimeWatched.SelectedDate.HasValue)
            {
                return;
            }

            _currentMovieRecord.Watched = LastTimeWatched.SelectedDate.Value.ToShortDateString();

            _movies.Update(_currentMovieRecord);
            MovieGrid.SelectedItem = null;
        }

        private void ToggleWatchedFlyoutClick(object sender, RoutedEventArgs e)
        {
            LastTimeWatched.SelectedDate = !string.IsNullOrWhiteSpace(_currentMovieRecord.Watched)
                ? DateTime.Parse(_currentMovieRecord.Watched)
                : DateTime.Now;

            WatchedFlyout.Header = $"last time of watching{Environment.NewLine}'{_currentMovieRecord.Name}'";
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