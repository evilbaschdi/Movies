using FluentAvalonia.UI.Windowing;
using Movie.AvaloniaUI.ViewModels;

namespace Movie.AvaloniaUI.Views;

/// <inheritdoc />
public partial class MainWindow : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MovieGrid_DoubleTapped(object sender, Avalonia.Input.TappedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.EditMovieCommand.Execute(default);
        }
    }
}
