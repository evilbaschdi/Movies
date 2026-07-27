using EvilBaschdi.Core.Avalonia.DependencyInjection;
using Movie.AvaloniaUI.Views;

namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="IWatchedReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class WatchedReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IWatchedReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var watchedDialog = ApplicationServices.GetRequiredService<WatchedDialog>();
        var viewModel = watchedDialog.DataContext as WatchedViewModel;
        if (viewModel is not null)
        {
            viewModel.LoadFromCurrentMovie();
        }

        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow is not null)
        {
            await watchedDialog.ShowDialog(mainWindow);
            var mainVm = mainWindow.DataContext as MainWindowViewModel;
            mainVm?.Load();
        }
    }
}