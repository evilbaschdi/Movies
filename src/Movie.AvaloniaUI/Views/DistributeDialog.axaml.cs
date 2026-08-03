using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Themes;
using FluentAvalonia.UI.Windowing;
using Movie.AvaloniaUI.ViewModels;

namespace Movie.AvaloniaUI.Views;

/// <inheritdoc />
public partial class LendDialog : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public LendDialog()
    {
        InitializeComponent();
        ThemeEngine.ApplyThemeToWindow(this, false);
        var vm = ApplicationServices.GetRequiredService<LendViewModel>();
        DataContext = vm;
        vm.CloseAction = Close;
    }
}