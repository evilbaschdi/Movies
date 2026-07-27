using Movie.Core;
using Movie.Core.Models;

namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="IGotBackReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class GotBackReactiveCommand(
    [NotNull] IMovies movies,
    [NotNull] ICurrentMovie currentMovie,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IGotBackReactiveCommand
{
    private readonly IMovies _movies = movies ?? throw new ArgumentNullException(nameof(movies));
    private readonly ICurrentMovie _currentMovie = currentMovie ?? throw new ArgumentNullException(nameof(currentMovie));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override Task RunAsync(CancellationToken cancellationToken = default)
    {
        var movie = _currentMovie.Value;
        if (movie is not null)
        {
            movie.Distributed = false;
            movie.DistributedTo = string.Empty;
            _movies.Update(movie);

            var mainWindow = _mainWindowByApplicationLifetime.Value;
            var mainVm = mainWindow?.DataContext as MainWindowViewModel;
            mainVm?.Load();
        }

        return Task.CompletedTask;
    }
}