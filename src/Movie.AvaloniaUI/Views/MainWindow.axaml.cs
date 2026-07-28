using Avalonia.Controls;
using Avalonia.Input;
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

        var groupByPanel = this.FindControl<Border>("GroupByPanel");

        if (groupByPanel == null)
        {
            return;
        }

        DragDrop.AddDropHandler(groupByPanel, OnColumnDropped);
        DragDrop.AddDragOverHandler(groupByPanel, (_, e) => e.DragEffects = DragDropEffects.Move);
    }

    private void MovieGrid_DoubleTapped(object sender, TappedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.EditMovieCommand.Execute(default);
        }
    }

    // ReSharper disable once AsyncVoidEventHandlerMethod
    private async void OnHeaderPointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is not TextBlock { Tag: string propertyName } textBlock)
        {
            return;
        }

        var headerTitle = textBlock.Text ?? propertyName;

        // Format: "PropertyName|HeaderTitle"
        var payload = $"{propertyName}|{headerTitle}";

        var dragData = new DataTransfer();
        dragData.Add(DataTransferItem.CreateText(payload));

        await DragDrop.DoDragDropAsync(e, dragData, DragDropEffects.Move);
    }

    private void OnColumnDropped(object sender, DragEventArgs e)
    {
        if (e.DataTransfer.TryGetText() is not { } textPayload || DataContext is not MainWindowViewModel vm)
        {
            return;
        }

        var parts = textPayload.Split('|');
        if (parts.Length != 2)
        {
            return;
        }

        var propertyName = parts[0];
        var headerTitle = parts[1];

        vm.AddGroup(propertyName, headerTitle);
    }
}