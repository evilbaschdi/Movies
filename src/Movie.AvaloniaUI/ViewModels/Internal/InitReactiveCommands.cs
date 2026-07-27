namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <inheritdoc />
public class InitReactiveCommands(
    [NotNull] IAboutWindowReactiveCommand aboutWindowReactiveCommand,
    [NotNull] IAddMovieReactiveCommand addMovieReactiveCommand,
    [NotNull] IDeleteMovieReactiveCommand deleteMovieReactiveCommand,
    [NotNull] IDistributeReactiveCommand distributeReactiveCommand,
    [NotNull] IEditMovieReactiveCommand editMovieReactiveCommand,
    [NotNull] IGotBackReactiveCommand gotBackReactiveCommand,
    [NotNull] ISettingsReactiveCommand settingsReactiveCommand,
    [NotNull] IWatchedReactiveCommand watchedReactiveCommand
) : IInitReactiveCommands
{
    /// <inheritdoc />
    public IAboutWindowReactiveCommand AboutWindowReactiveCommand { get; } = aboutWindowReactiveCommand ?? throw new ArgumentNullException(nameof(aboutWindowReactiveCommand));

    /// <inheritdoc />
    public IAddMovieReactiveCommand AddMovieReactiveCommand { get; } = addMovieReactiveCommand ?? throw new ArgumentNullException(nameof(addMovieReactiveCommand));

    /// <inheritdoc />
    public IDeleteMovieReactiveCommand DeleteMovieReactiveCommand { get; } = deleteMovieReactiveCommand ?? throw new ArgumentNullException(nameof(deleteMovieReactiveCommand));

    /// <inheritdoc />
    public IDistributeReactiveCommand DistributeReactiveCommand { get; } = distributeReactiveCommand ?? throw new ArgumentNullException(nameof(distributeReactiveCommand));

    /// <inheritdoc />
    public IEditMovieReactiveCommand EditMovieReactiveCommand { get; } = editMovieReactiveCommand ?? throw new ArgumentNullException(nameof(editMovieReactiveCommand));

    /// <inheritdoc />
    public IGotBackReactiveCommand GotBackReactiveCommand { get; } = gotBackReactiveCommand ?? throw new ArgumentNullException(nameof(gotBackReactiveCommand));

    /// <inheritdoc />
    public ISettingsReactiveCommand SettingsReactiveCommand { get; } = settingsReactiveCommand ?? throw new ArgumentNullException(nameof(settingsReactiveCommand));

    /// <inheritdoc />
    public IWatchedReactiveCommand WatchedReactiveCommand { get; } = watchedReactiveCommand ?? throw new ArgumentNullException(nameof(watchedReactiveCommand));
}
