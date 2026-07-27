namespace Movie.AvaloniaUI.ViewModels.Internal;

/// <summary />
public interface IInitReactiveCommands
{
    /// <summary />
    IAboutWindowReactiveCommand AboutWindowReactiveCommand { get; }

    /// <summary />
    IAddMovieReactiveCommand AddMovieReactiveCommand { get; }

    /// <summary />
    IDeleteMovieReactiveCommand DeleteMovieReactiveCommand { get; }

    /// <summary />
    IDistributeReactiveCommand DistributeReactiveCommand { get; }

    /// <summary />
    IEditMovieReactiveCommand EditMovieReactiveCommand { get; }

    /// <summary />
    IGotBackReactiveCommand GotBackReactiveCommand { get; }

    /// <summary />
    ISettingsReactiveCommand SettingsReactiveCommand { get; }

    /// <summary />
    IWatchedReactiveCommand WatchedReactiveCommand { get; }
}
