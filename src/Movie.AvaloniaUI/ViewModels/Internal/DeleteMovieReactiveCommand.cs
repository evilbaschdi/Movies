using FluentAvalonia.UI.Controls;
using Movie.Core;
using Movie.Core.Models;

namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="IDeleteMovieReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class DeleteMovieReactiveCommand(
    [NotNull] IMovies movies,
    [NotNull] ICurrentMovie currentMovie,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IDeleteMovieReactiveCommand
{
    private readonly IMovies _movies = movies ?? throw new ArgumentNullException(nameof(movies));
    private readonly ICurrentMovie _currentMovie = currentMovie ?? throw new ArgumentNullException(nameof(currentMovie));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var movie = _currentMovie.Value;
        if (movie is null)
        {
            return;
        }

        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow is null)
        {
            return;
        }

        var dialog = new FAContentDialog
                     {
                         Title = "Delete",
                         Content = $"You are about to delete '{movie.Name}'",
                         PrimaryButtonText = "Delete",
                         CloseButtonText = "Cancel",
                         DefaultButton = FAContentDialogButton.Close
                     };

        var result = await dialog.ShowAsync(mainWindow);
        if (result == FAContentDialogResult.Primary)
        {
            try
            {
                _movies.Delete(movie.Id);
            }
            catch (Exception ex)
            {
                var errorDialog = new FATaskDialog
                                  {
                                      Title = "Delete failed",
                                      Content = $"failed to delete record {movie.Name.Trim()} from database\n Message : {ex.Message}",
                                      IconSource = new FASymbolIconSource { Symbol = FASymbol.AlertUrgentFilled },
                                      Buttons = { FATaskDialogButton.OKButton },
                                      XamlRoot = mainWindow
                                  };
                await errorDialog.ShowAsync();
            }

            var mainVm = mainWindow.DataContext as MainWindowViewModel;
            mainVm?.Load();
        }
    }
}