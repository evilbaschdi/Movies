using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Themes;
using FluentAvalonia.UI.Windowing;
using Movie.AvaloniaUI.ViewModels;

namespace Movie.AvaloniaUI.Views;

/// <inheritdoc />
public partial class DistributeDialog : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public DistributeDialog()
    {
        InitializeComponent();
        ThemeEngine.ApplyThemeToWindow(this, false);
        var vm = ApplicationServices.GetRequiredService<DistributeViewModel>();
        DataContext = vm;
        vm.CloseAction = () => Close();
    }
}
