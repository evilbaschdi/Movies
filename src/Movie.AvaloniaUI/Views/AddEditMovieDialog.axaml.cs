using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Themes;
using FluentAvalonia.UI.Windowing;
using Movie.AvaloniaUI.ViewModels;

namespace Movie.AvaloniaUI.Views;

/// <inheritdoc />
public partial class AddEditMovieDialog : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditMovieDialog()
    {
        InitializeComponent();
        ThemeEngine.ApplyThemeToWindow(this, false);
        var vm = ApplicationServices.GetRequiredService<AddEditMovieViewModel>();
        DataContext = vm;
        vm.CloseAction = Close;
    }
}