using ReactiveUI;
using Movie.Core;
using Movie.Core.Models;

namespace Movie.AvaloniaUI.ViewModels;

/// <inheritdoc />
public class DistributeViewModel : ViewModelBase
{
    private readonly IMovies _movies;
    private readonly ICurrentMovie _currentMovie;

    private string _movieName = string.Empty;
    private string _distributedTo = string.Empty;

    /// <summary>
    ///     Constructor
    /// </summary>
    public DistributeViewModel(IMovies movies, ICurrentMovie currentMovie)
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
    public string DistributedTo
    {
        get => _distributedTo;
        set => this.RaiseAndSetIfChanged(ref _distributedTo, value);
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
            DistributedTo = movie.DistributedTo ?? string.Empty;
        }
    }

    private void Save()
    {
        if (string.IsNullOrWhiteSpace(DistributedTo))
        {
            return;
        }

        var movie = _currentMovie.Value;
        if (movie is not null)
        {
            movie.Distributed = true;
            movie.DistributedTo = DistributedTo;
            _movies.Update(movie);
        }

        CloseAction?.Invoke();
    }

    private void Cancel()
    {
        CloseAction?.Invoke();
    }
}
