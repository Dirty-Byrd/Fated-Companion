using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Styling;

namespace Fated_Companion.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MinimizeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Normal)
        {
            this.WindowState = WindowState.Maximized;
            //windowBTNS.Margin = new Thickness(0, 6, 6, 0);
            //maximizeBTN.Style = (Style)Resources["maxBTN"];
        }
        else if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            //windowBTNS.Margin = new Thickness(0, 0, 0, 0);
            //maximizeBTN.Style = (Style)Resources["normBTN"];
        }
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void Border_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        BeginMoveDrag(e);
    }
}
