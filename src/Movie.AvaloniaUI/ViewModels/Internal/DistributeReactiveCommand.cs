using EvilBaschdi.Core.Avalonia.DependencyInjection;
using Movie.AvaloniaUI.Views;

namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="ILendReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class LendReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    : ReactiveCommandRxVoidTask, ILendReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var lendDialog = ApplicationServices.GetRequiredService<LendDialog>();
        var viewModel = lendDialog.DataContext as LendViewModel;
        if (viewModel is not null)
        {
            viewModel.LoadFromCurrentMovie();
        }

        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow is not null)
        {
            await lendDialog.ShowDialog(mainWindow);
            var mainVm = mainWindow.DataContext as MainWindowViewModel;
            mainVm?.Load();
        }
    }
}