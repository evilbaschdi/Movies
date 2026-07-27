using Avalonia;
using EvilBaschdi.About.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.AppBuilderImplementations;
using Movie.Core.DependencyInjection;
using Movie.AvaloniaUI.DependencyInjection;

namespace Movie.AvaloniaUI;

internal class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => new AppBuilderImplementationToUseReactiveUIWithMicrosoftDependencyResolver<App>()
           .ValueFor(serviceCollection =>
                     {
                         serviceCollection.AddCoreServices();
                         serviceCollection.AddAboutServices();
                         serviceCollection.AddAvaloniaServices();
                         serviceCollection.AddReactiveCommandServices();
                         serviceCollection.AddWindowsAndViewModels();
                     })
#if DEBUG
           .WithDeveloperTools()
#endif
    ;
}
