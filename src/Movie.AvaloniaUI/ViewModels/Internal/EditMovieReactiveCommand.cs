using EvilBaschdi.Core.Avalonia.DependencyInjection;
using Movie.AvaloniaUI.Views;

namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="IEditMovieReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class EditMovieReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IEditMovieReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var addEditMovieDialog = ApplicationServices.GetRequiredService<AddEditMovieDialog>();
        var viewModel = addEditMovieDialog.DataContext as AddEditMovieViewModel;
        if (viewModel is not null)
        {
            viewModel.Mode = "edit";
            viewModel.LoadFromCurrentMovie();
        }

        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow is not null)
        {
            await addEditMovieDialog.ShowDialog(mainWindow);
            var mainVm = mainWindow.DataContext as MainWindowViewModel;
            mainVm?.Load();
        }
    }
}