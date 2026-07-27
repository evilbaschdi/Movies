using ReactiveUI;
using Movie.Core;
using Movie.Core.Models;

namespace Movie.AvaloniaUI.ViewModels;

/// <inheritdoc />
public class WatchedViewModel : ViewModelBase
{
    private readonly IMovies _movies;
    private readonly ICurrentMovie _currentMovie;

    private string _movieName = string.Empty;
    private DateTimeOffset? _watchedDate = DateTimeOffset.Now;

    /// <summary>
    ///     Constructor
    /// </summary>
    public WatchedViewModel(IMovies movies, ICurrentMovie currentMovie)
    {
        _movies = movies ?? throw new ArgumentNullException(nameof(movies));
        _currentMovie = currentMovie ?? throw new ArgumentNullException(nameof(currentMovie));

        SaveCommand = ReactiveCommand.Create(Save);
        CancelCommand = ReactiveCommand.Create(Cancel);
    }

    /// <summary />
    public Action CloseAction { get; set; }

    /// <summary />
    public string MovieName
    {
        get => _movieName;
        set => this.RaiseAndSetIfChanged(ref _movieName, value);
    }

    /// <summary />
    public DateTimeOffset? WatchedDate
    {
        get => _watchedDate;
        set => this.RaiseAndSetIfChanged(ref _watchedDate, value);
    }

    /// <summary />
    public IDisposable SaveCommand { get; }

    /// <summary />
    public IDisposable CancelCommand { get; }

    /// <summary />
    public void LoadFromCurrentMovie()
    {
        var movie = _currentMovie.Value;
        if (movie is not null)
        {
            MovieName = movie.Name ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(movie.Watched) && DateTime.TryParse(movie.Watched, out var parsedDate))
            {
                WatchedDate = parsedDate;
            }
            else
            {
                WatchedDate = DateTimeOffset.Now;
            }
        }
    }

    private void Save()
    {
        if (!WatchedDate.HasValue)
        {
            return;
        }

        var movie = _currentMovie.Value;
        if (movie is not null)
        {
            movie.Watched = WatchedDate.Value.DateTime.ToShortDateString();
            _movies.Update(movie);
        }

        CloseAction?.Invoke();
    }

    private void Cancel()
    {
        CloseAction?.Invoke();
    }
}
