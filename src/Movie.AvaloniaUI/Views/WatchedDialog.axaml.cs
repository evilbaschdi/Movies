using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Themes;
using FluentAvalonia.UI.Windowing;
using Movie.AvaloniaUI.ViewModels;

namespace Movie.AvaloniaUI.Views;

/// <inheritdoc />
public partial class WatchedDialog : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public WatchedDialog()
    {
        InitializeComponent();
        ThemeEngine.ApplyThemeToWindow(this, false);
        var vm = ApplicationServices.GetRequiredService<WatchedViewModel>();
        DataContext = vm;
        vm.CloseAction = () => Close();
    }
}
