using FluentAvalonia.UI.Controls;
using Movie.Core;
using Movie.Core.Models;
using ReactiveUI;

namespace Movie.AvaloniaUI.ViewModels;

/// <inheritdoc />
public class AddEditMovieViewModel : ViewModelBase
{
    private readonly IMovies _movies;
    private readonly ISettings _settings;
    private readonly ICurrentMovie _currentMovie;
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;

    private string _mode = "add";
    private string _movieName = string.Empty;
    private int _year = DateTime.Now.Year;
    private string _format = string.Empty;

    private static readonly List<string> MovieFormats = new() { "DVD", "Blu-ray", "VHS", "MP4", "MKV", "MPEG", "AVI", "ISO", "FLV", "OGG" };
    private static readonly List<string> MusicFormats = new() { "CD", "MP3", "Kassette", "Schallplatte" };

    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditMovieViewModel(
        IMovies movies,
        ISettings settings,
        ICurrentMovie currentMovie,
        IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    {
        _movies = movies ?? throw new ArgumentNullException(nameof(movies));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _currentMovie = currentMovie ?? throw new ArgumentNullException(nameof(currentMovie));
        _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
        SaveAndNewCommand = ReactiveCommand.CreateFromTask(SaveAndNewAsync);
        CancelCommand = ReactiveCommand.Create(Cancel);
    }

    /// <summary />
    public Action CloseAction { get; set; }

    /// <summary />
    public string Mode
    {
        get => _mode;
        set
        {
            this.RaiseAndSetIfChanged(ref _mode, value);
            this.RaisePropertyChanged(nameof(TitleText));
        }
    }

    /// <summary />
    public string MovieName
    {
        get => _movieName;
        set => this.RaiseAndSetIfChanged(ref _movieName, value);
    }

    /// <summary />
    public int Year
    {
        get => _year;
        set => this.RaiseAndSetIfChanged(ref _year, value);
    }

    /// <summary />
    public string Format
    {
        get => _format;
        set => this.RaiseAndSetIfChanged(ref _format, value);
    }

    /// <summary />
    public List<string> Formats => _settings.DbType == "music" ? MusicFormats : MovieFormats;

    /// <summary />
    public string TitleText => Mode == "add" ? $"add new {_settings.DbType}" : $"edit {_settings.DbType}";

    /// <summary />
    public IDisposable SaveCommand { get; }

    /// <summary />
    public IDisposable SaveAndNewCommand { get; }

    /// <summary />
    public IDisposable CancelCommand { get; }

    /// <summary />
    public void LoadFromCurrentMovie()
    {
        var movie = _currentMovie.Value;
        if (movie is not null && Mode == "edit")
        {
            MovieName = movie.Name ?? string.Empty;
            Year = movie.Year;
            Format = movie.Format ?? string.Empty;
        }
        else
        {
            ClearForm();
        }
    }

    /// <summary />
    public void ClearForm()
    {
        MovieName = string.Empty;
        Year = DateTime.Now.Year;
        Format = string.Empty;
    }

    private async Task SaveAsync()
    {
        if (await ValidateAndSaveAsync())
        {
            CloseAction?.Invoke();
        }
    }

    private async Task SaveAndNewAsync()
    {
        if (await ValidateAndSaveAsync())
        {
            ClearForm();
        }
    }

    private void Cancel()
    {
        CloseAction?.Invoke();
    }

    private async Task<bool> ValidateAndSaveAsync()
    {
        if (string.IsNullOrWhiteSpace(MovieName))
        {
            return false;
        }

        if (Mode == "add" && _movies.ValueByName(MovieName) != null)
        {
            var mainWindow = _mainWindowByApplicationLifetime.Value;
            if (mainWindow is not null)
            {
                var dialog = new FATaskDialog
                             {
                                 Title = "Already existing!",
                                 Content = $"'{MovieName}' already exists in the database.",
                                 IconSource = new FASymbolIconSource { Symbol = FASymbol.AlertUrgentFilled },
                                 Buttons = { FATaskDialogButton.OKButton },
                                 XamlRoot = mainWindow
                             };
                await dialog.ShowAsync();
            }

            return false;
        }

        if (Mode == "add")
        {
            var newMovie = new MovieRecord
                           {
                               Id = Guid.NewGuid().ToString(),
                               Name = MovieName,
                               Year = Year,
                               Format = Format
                           };
            _movies.Create(newMovie);
        }
        else
        {
            var current = _currentMovie.Value;
            if (current is not null)
            {
                current.Name = MovieName;
                current.Year = Year;
                current.Format = Format;
                _movies.Update(current);
            }
        }

        return true;
    }
}