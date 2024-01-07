using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using EvilBaschdi.About.Core;
using EvilBaschdi.About.Core.Models;
using EvilBaschdi.About.Wpf;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.FlyOut;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;
using Movie.Core.Models;
using Movie.Internal;

namespace Movie;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class MainWindow
{
    private readonly IAddEdit _addEdit;
    private readonly IAppBasic _appBasic;
    private readonly ICurrentFlyOuts _currentFlyOuts;
    private readonly IMovies _movies;
    private readonly ISettings _settings;
    private readonly IToggleFlyOut _toggleFlyOut;
    private string _currentId;
    private IMovieRecord _currentMovieRecord;
    private string _dbType;
    private string _exception;
    private ListCollectionView _listCollectionView;
    private string _prevSortHeader;
    private SortDescription _sd;
    private string _sortHeader;

    /// <summary>
    ///     MainWindows.
    ///     Gets a new movie list instance.
    /// </summary>
    public MainWindow()
    {
        IApplicationSettingsFromJsonFile applicationSettingsFromJsonFile = new ApplicationSettingsFromJsonFile();
        _settings = new Settings(applicationSettingsFromJsonFile);
        _appBasic = new AppBasic(this, _settings);
        IXmlDatabase xmlDatabase = new XmlDatabase(_settings);
        ITransformDataRowToMovieRecord transformDataRowToMovieRecord = new TransformDataRowToMovieRecord();
        _movies = new Movies(xmlDatabase, transformDataRowToMovieRecord);

        InitializeComponent();

        IApplicationStyle applicationStyle = new ApplicationStyle();
        IApplicationLayout applicationLayout = new ApplicationLayout();
        applicationStyle.Run();
        applicationLayout.RunFor((true, false));
        _addEdit = new AddEdit(this, _movies);
        ValidateSettings();
        _appBasic.SetComboBoxItems();
        _currentFlyOuts = new CurrentFlyOuts();
        _toggleFlyOut = new ToggleFlyOut();
    }

    private void AboutWindowClick(object sender, RoutedEventArgs e)
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutViewModel aboutModel = new AboutViewModel(aboutContent);
        IApplyMicaBrush applyMicaBrush = new ApplyMicaBrush();
        IApplicationLayout applicationLayout = new ApplicationLayout();
        var aboutWindow = new AboutWindow(aboutModel, applicationLayout, applyMicaBrush);

        aboutWindow.ShowDialog();
    }

    #region DataGrid Logic

    /// <summary>
    ///     (Re)Loads movie data source to grid ans calls sorting.
    /// </summary>
    private void Load()
    {
        var movies = _movies.Value;
        _listCollectionView = new(movies);
        _sd = new("Name", ListSortDirection.Ascending);
        _listCollectionView.SortDescriptions.Add(_sd);
        MovieGrid.SetCurrentValue(ItemsControl.ItemsSourceProperty, _listCollectionView);
    }

    private void DataGridSorting(object sender, DataGridSortingEventArgs e)
    {
        _sortHeader = e.Column.SortMemberPath;

        _sd = _sortHeader == _prevSortHeader
            ? new(_sortHeader, ListSortDirection.Descending)
            : new SortDescription(_sortHeader, ListSortDirection.Ascending);
        _prevSortHeader = _sortHeader;
    }

    private void MovieGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MovieGrid.SelectedItem == null)
        {
            return;
        }

        _currentMovieRecord = (MovieRecord)MovieGrid.SelectedItem;

        _currentId = _currentMovieRecord.Id;
        // ReSharper disable once UnusedVariable
        //todo: repair
        var distributed = _currentMovieRecord.Distributed;

        //DistributeMenuItem.SetCurrentValue(IsEnabledProperty, distributed != "True");
        //GotBackMenuItem.SetCurrentValue(IsEnabledProperty, distributed == "True");
    }

    #endregion DataGrid Logic

    #region Settings

    private void ValidateSettings()
    {
        Year.SetCurrentValue(NumericUpDown.MaximumProperty, (double)DateTime.Now.Year);
        _dbType = _settings.DbType == "music" ? "music" : "movie";
        DbType.SetCurrentValue(ComboBox.TextProperty, !string.IsNullOrWhiteSpace(_settings.DbType) ? _settings.DbType : "movie");
        SetCurrentValue(TitleProperty, _dbType);
        NewContent.SetCurrentValue(TextBlock.TextProperty, $"add new {_dbType}");

        if (!string.IsNullOrWhiteSpace(_settings.FilePath))
        {
            SearchCategory.SetCurrentValue(IsEnabledProperty, true);
            SearchFilter.SetCurrentValue(IsEnabledProperty, true);
            New.SetCurrentValue(IsEnabledProperty, true);
            SearchCategory.SetCurrentValue(ComboBox.TextProperty, "Name");
            DbPath.SetCurrentValue(TextBox.TextProperty, _settings.FilePath);
            if (!File.Exists(DbPath.Text))
            {
                DbPath.SetCurrentValue(BackgroundProperty, Brushes.Maroon);
            }

            Load();
        }
        else
        {
            SearchCategory.SetCurrentValue(IsEnabledProperty, false);
            SearchFilter.SetCurrentValue(IsEnabledProperty, false);
            New.SetCurrentValue(IsEnabledProperty, false);
        }
    }

    private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
    {
        ToggleFlyOut(0);
    }

    private void SaveSettingsClick(object sender, RoutedEventArgs e)
    {
        _appBasic.Save();
        Load();
    }

    private void BrowseClick(object sender, RoutedEventArgs e)
    {
        _appBasic.Browse();
    }

    private void ResetClick(object sender, RoutedEventArgs e)
    {
        _appBasic.Reset();
    }

    private void ToggleFlyOut(int index, bool stayOpen = false)
    {
        // ReSharper disable once UseNullPropagation
        if (Flyouts == null)
        {
            return;
        }

        var currentFlyOutsModel = _currentFlyOuts.ValueFor(Flyouts, index);
        _toggleFlyOut.RunFor(currentFlyOutsModel, stayOpen);

        var activeFlyOut = currentFlyOutsModel?.ActiveFlyOut;
        if (activeFlyOut == null)
        {
            return;
        }

        activeFlyOut.ClosingFinished += ActiveFlyOutClosingFinished;
    }

    private void ActiveFlyOutClosingFinished(object sender, RoutedEventArgs e)
    {
        Load();
    }

    #endregion Settings

    #region Add Edit Movie

    private void NewClick(object sender, RoutedEventArgs e)
    {
        _currentId = null;
        _addEdit.Mode = "add";
        AddEditFlyout.SetCurrentValue(HeaderedContentControl.HeaderProperty, $"add new {_dbType}");
        Year.SetCurrentValue(NumericUpDown.ValueProperty, Year.Maximum);
        _currentId = string.Empty;
        ToggleFlyOut(1);
    }

    private void Edit()
    {
        _addEdit.Mode = "edit";
        AddEditFlyout.SetCurrentValue(HeaderedContentControl.HeaderProperty, $"edit {_dbType}");
        LoadCurrentMovieData();
        ToggleFlyOut(1);
    }

    private void LoadCurrentMovieData()
    {
        _currentMovieRecord = _movies.ValueById(_currentId);
        if (_currentMovieRecord == null)
        {
            return;
        }

        MovieName.SetCurrentValue(TextBox.TextProperty, _currentMovieRecord.Name);
        Year.SetCurrentValue(NumericUpDown.ValueProperty, string.IsNullOrWhiteSpace(_currentMovieRecord.Year)
            ? Year.Maximum
            : Convert.ToDouble(_currentMovieRecord.Year));
        Format.SetCurrentValue(ComboBox.TextProperty, _currentMovieRecord.Format);
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
    private void NewEntry()
    {
        ClearForm();
    }

    private void ClearForm()
    {
        _currentMovieRecord = null;
        MovieName.Focus();
        MovieName.SetCurrentValue(TextBox.TextProperty, "");
        Year.SetCurrentValue(NumericUpDown.ValueProperty, Year.Maximum);
        Format.SetCurrentValue(ComboBox.TextProperty, "");
    }

    private void CancelClick(object sender, RoutedEventArgs routedEventArgs)
    {
        CleanupAndClose();
    }

    /// <summary>
    /// </summary>
    private void CleanupAndClose()
    {
        MovieName.Clear();
        _currentId = null;
        ToggleFlyOut(1);
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

        Load();
    }

    #endregion Delete Movie

    #region Distribution

    private void DistributeClick(object sender, RoutedEventArgs e)
    {
        DistributedFlyout.SetCurrentValue(HeaderedContentControl.HeaderProperty, $"distribute{Environment.NewLine}'{_currentMovieRecord.Name}'");
        ToggleFlyOut(3, true);
    }

    private void DistributeCheckBoxClick(object sender, RoutedEventArgs e)
    {
        var result = ((CheckBox)sender).IsChecked;

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
        MovieGrid.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, null);
    }

    private void GotBackClick(object sender, RoutedEventArgs e)
    {
        _currentMovieRecord.Distributed = "False";
        _currentMovieRecord.DistributedTo = "";
        _movies.Update(_currentMovieRecord);
        MovieGrid.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, null);
    }

    #endregion Distribution

    #region Search

    private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
    {
        _listCollectionView.Filter = m => Filter(m, SearchCategory.Text, SearchFilter.Text);
    }

    private static bool Filter(object m, string category, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        var movieRecord = (MovieRecord)m;

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
        SearchFilter.SetCurrentValue(TextBox.MaxLengthProperty, 1);
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
        MovieGrid.SetCurrentValue(System.Windows.Controls.Primitives.Selector.SelectedItemProperty, null);
    }

    private void ToggleWatchedFlyoutClick(object sender, RoutedEventArgs e)
    {
        LastTimeWatched.SetCurrentValue(DatePicker.SelectedDateProperty, !string.IsNullOrWhiteSpace(_currentMovieRecord.Watched)
            ? DateTime.Parse(_currentMovieRecord.Watched)
            : DateTime.Now);

        WatchedFlyout.SetCurrentValue(HeaderedContentControl.HeaderProperty, $"last time of watching{Environment.NewLine}'{_currentMovieRecord.Name}'");
        ToggleFlyOut(2, true);
    }

    private void CommandBindingCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }

    private void CommandBindingExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        ((Calendar)e.Parameter).SetCurrentValue(Calendar.SelectedDateProperty, DateTime.Now.Date);
    }

    #endregion Watched movie
}