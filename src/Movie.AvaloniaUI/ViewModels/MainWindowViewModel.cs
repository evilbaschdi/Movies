using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Platform.Storage;
using Movie.AvaloniaUI.ViewModels.Internal;
using Movie.Core;
using Movie.Core.Models;
using ReactiveUI;

namespace Movie.AvaloniaUI.ViewModels;

/// <inheritdoc />
public class MainWindowViewModel : ViewModelBase
{
    private readonly IMovies _movies;
    private readonly ISettings _settings;
    private readonly ICurrentMovie _currentMovie;
    private readonly IInitReactiveCommands _initReactiveCommands;
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;

    private string _dbType;
    private string _dbPath;

    /// <summary>
    ///     Represents a group chip for filtering movies.
    /// </summary>
    /// <param name="PropertyName"></param>
    /// <param name="DisplayName"></param>
    public record GroupChip(string PropertyName, string DisplayName);

    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindowViewModel(
        IMovies movies,
        ISettings settings,
        ICurrentMovie currentMovie,
        IInitReactiveCommands initReactiveCommands,
        IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    {
        _movies = movies ?? throw new ArgumentNullException(nameof(movies));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _currentMovie = currentMovie ?? throw new ArgumentNullException(nameof(currentMovie));
        _initReactiveCommands = initReactiveCommands ?? throw new ArgumentNullException(nameof(initReactiveCommands));
        _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

        _dbType = !string.IsNullOrWhiteSpace(_settings.DbType) ? _settings.DbType : "movie";
        _dbPath = _settings.FilePath ?? string.Empty;
        RemoveGroupCommand = ReactiveCommand.Create<GroupChip>(RemoveGroup);

        Run();
    }

    /// <summary />
    public void Run()
    {
        AboutWindowCommand = _initReactiveCommands.AboutWindowReactiveCommand.Command;
        AddMovieCommand = _initReactiveCommands.AddMovieReactiveCommand.Command;
        DeleteMovieCommand = _initReactiveCommands.DeleteMovieReactiveCommand.Command;
        DistributeCommand = _initReactiveCommands.DistributeReactiveCommand.Command;
        EditMovieCommand = _initReactiveCommands.EditMovieReactiveCommand.Command;
        GotBackCommand = _initReactiveCommands.GotBackReactiveCommand.Command;
        SettingsCommand = _initReactiveCommands.SettingsReactiveCommand.Command;
        WatchedCommand = _initReactiveCommands.WatchedReactiveCommand.Command;

        Load();
    }

    /// <summary />
    public DataGridCollectionView DataGridCollectionViewMovies
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    /// <summary />
    public IMovieRecord SelectedMovie
    {
        get => _currentMovie.Value;
        set
        {
            _currentMovie.Value = value;
            this.RaisePropertyChanged();
            this.RaisePropertyChanged(nameof(IsMovieSelected));
            this.RaisePropertyChanged(nameof(IsDistributed));
            this.RaisePropertyChanged(nameof(IsNotDistributed));
        }
    }

    /// <summary />
    public bool IsMovieSelected => SelectedMovie != null;

    /// <summary />
    public bool IsDistributed => SelectedMovie?.Distributed ?? false;

    /// <summary />
    public bool IsNotDistributed => SelectedMovie is { Distributed: false };

    /// <summary />
    public string SearchFilterText
    {
        get;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            DataGridCollectionViewMovies?.Refresh();
        }
    } = string.Empty;

    /// <summary />
    public string SearchCategory
    {
        get;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            DataGridCollectionViewMovies?.Refresh();
        }
    } = "Name";

    /// <summary />
    public List<string> SearchCategoryItems { get; } = ["Name", "Year", "Format", "Distributed"];

    /// <summary />
    public string DbType
    {
        get => _dbType;
        set => this.RaiseAndSetIfChanged(ref _dbType, value);
    }

    /// <summary />
    public string DbPath
    {
        get => _dbPath;
        set => this.RaiseAndSetIfChanged(ref _dbPath, value);
    }

    /// <summary />
    public string TitleText => _dbType;

    /// <summary />
    public string NewContentText => $"add new {_dbType}";

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> AboutWindowCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> AddMovieCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> DeleteMovieCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> DistributeCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> EditMovieCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> GotBackCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> SettingsCommand { get; set; }

    /// <summary />
    public ReactiveCommand<RxVoid, RxVoid> WatchedCommand { get; set; }

    /// <summary />
    public void Load()
    {
        _dbType = !string.IsNullOrWhiteSpace(_settings.DbType) ? _settings.DbType : "movie";
        _dbPath = _settings.FilePath ?? string.Empty;

        this.RaisePropertyChanged(nameof(DbType));
        this.RaisePropertyChanged(nameof(DbPath));
        this.RaisePropertyChanged(nameof(TitleText));
        this.RaisePropertyChanged(nameof(NewContentText));

        if (!string.IsNullOrWhiteSpace(_settings.FilePath) && File.Exists(_settings.FilePath))
        {
            var list = _movies.Value;
            DataGridCollectionViewMovies = new DataGridCollectionView(list);
            DataGridCollectionViewMovies.Filter = ValueFilter;
        }
        else
        {
            DataGridCollectionViewMovies = new DataGridCollectionView(new List<MovieRecord>());
        }
    }

    /// <summary />
    public async Task BrowseSettingsAsync()
    {
        var window = _mainWindowByApplicationLifetime.Value;
        if (window is null)
        {
            return;
        }

        var options = new FilePickerOpenOptions
                      {
                          Title = "Open XML Database",
                          FileTypeFilter =
                          [
                              new("XML Files") { Patterns = ["*.xml"] }
                          ],
                          AllowMultiple = false
                      };

        var result = await window.StorageProvider.OpenFilePickerAsync(options);
        if (result.Count > 0)
        {
            DbPath = result[0].Path.LocalPath;
        }
    }

    /// <summary />
    public void SaveSettings()
    {
        _settings.DbType = DbType;
        _settings.FilePath = DbPath;
        Load();
    }

    /// <summary />
    public async Task ResetSettingsAsync()
    {
        var window = _mainWindowByApplicationLifetime.Value;
        if (window is null)
        {
            return;
        }

        var options = new FilePickerSaveOptions
                      {
                          Title = "Save XML Database",
                          DefaultExtension = "xml",
                          SuggestedFileName = "Movie.xml",
                          FileTypeChoices =
                          [
                              new("XML Files") { Patterns = ["*.xml"] }
                          ]
                      };

        var result = await window.StorageProvider.SaveFilePickerAsync(options);
        if (result is not null)
        {
            var filePath = result.Path.LocalPath;
            DbPath = filePath;
            _settings.DbType = DbType;
            _settings.FilePath = filePath;

            await File.WriteAllTextAsync(filePath, "{\n  \"movies\": []\n}");
            Load();
        }
    }

    private bool ValueFilter(object m)
    {
        if (string.IsNullOrWhiteSpace(SearchFilterText))
        {
            return true;
        }

        var movieRecord = (MovieRecord)m;
        var text = SearchFilterText;

        return SearchCategory switch
        {
            "Name" => movieRecord.Name?.Contains(text, StringComparison.InvariantCultureIgnoreCase) ?? false,
            "Year" => movieRecord.Year.ToString().Contains(text, StringComparison.InvariantCultureIgnoreCase),
            "Format" => movieRecord.Format?.Contains(text, StringComparison.InvariantCultureIgnoreCase) ?? false,
            "Distributed" => movieRecord.Distributed.ToString().Contains(text, StringComparison.InvariantCultureIgnoreCase),
            _ => true
        };
    }

    /// <summary>
    ///     Gets the collection of active group chips.
    /// </summary>
    public ObservableCollection<GroupChip> ActiveGroupChips { get; } = [];

    /// <summary>
    ///     Gets the command to remove a group chip.
    /// </summary>
    public ReactiveCommand<GroupChip, RxVoid> RemoveGroupCommand { get; }

    /// <summary>
    ///     Adds a new group chip to the collection and updates the DataGridCollectionViewMovies with the corresponding
    ///     group description.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="displayName"></param>
    public void AddGroup(string propertyName, string displayName)
    {
        if (ActiveGroupChips.Any(c => c.PropertyName == propertyName))
        {
            return;
        }

        ActiveGroupChips.Add(new GroupChip(propertyName, displayName));

        DataGridCollectionViewMovies.GroupDescriptions.Add(new DataGridPathGroupDescription(propertyName));
    }

    private void RemoveGroup(GroupChip chip)
    {
        if (chip == null)
        {
            return;
        }

        ActiveGroupChips.Remove(chip);

        var description = DataGridCollectionViewMovies.GroupDescriptions
                                                      .OfType<DataGridPathGroupDescription>()
                                                      .FirstOrDefault(d => d.PropertyName == chip.PropertyName);

        if (description != null)
        {
            DataGridCollectionViewMovies.GroupDescriptions.Remove(description);
        }
    }
}