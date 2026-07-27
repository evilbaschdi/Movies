namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc cref="ISettingsReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class SettingsReactiveCommand : ReactiveCommandRxVoidTask, ISettingsReactiveCommand
{
    /// <inheritdoc />
    public override Task RunAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
