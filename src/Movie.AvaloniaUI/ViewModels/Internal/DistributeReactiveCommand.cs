using EvilBaschdi.Core.Avalonia.DependencyInjection;
using Movie.AvaloniaUI.Views;

namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="IDistributeReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class DistributeReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IDistributeReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var distributeDialog = ApplicationServices.GetRequiredService<DistributeDialog>();
        var viewModel = distributeDialog.DataContext as DistributeViewModel;
        if (viewModel is not null)
        {
            viewModel.LoadFromCurrentMovie();
        }

        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow is not null)
        {
            await distributeDialog.ShowDialog(mainWindow);
            var mainVm = mainWindow.DataContext as MainWindowViewModel;
            mainVm?.Load();
        }
    }
}