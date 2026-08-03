using ReactiveUI;
using Movie.Core;
using Movie.Core.Models;

namespace Movie.AvaloniaUI.ViewModels;

/// <inheritdoc />
public class LendViewModel : ViewModelBase
{
    private readonly IMovies _movies;
    private readonly ICurrentMovie _currentMovie;

    private string _movieName = string.Empty;
    private string _lentTo = string.Empty;

    /// <summary>
    ///     Constructor
    /// </summary>
    public LendViewModel(IMovies movies, ICurrentMovie currentMovie)
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
    public string LentTo
    {
        get => _lentTo;
        set => this.RaiseAndSetIfChanged(ref _lentTo, value);
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
            LentTo = movie.LentTo ?? string.Empty;
        }
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(LentTo))
        {
            return;
        }

        var movie = _currentMovie.Value;
        if (movie is not null)
        {
            movie.Lent = true;
            movie.LentTo = LentTo;
            _movies.Update(movie);
        }

        CloseAction?.Invoke();
    }

    private void Cancel()
    {
        CloseAction?.Invoke();
    }
}
