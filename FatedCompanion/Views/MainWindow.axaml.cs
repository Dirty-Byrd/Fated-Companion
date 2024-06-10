using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Styling;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Fated_Companion.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        normWindowMaxButton.IsVisible = true;
        maxWindowMaxButton.IsVisible = false;
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
            normWindowMaxButton.IsVisible = false;
            maxWindowMaxButton.IsVisible = true;
        }
        else if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            //windowBTNS.Margin = new Thickness(0, 0, 0, 0);
            normWindowMaxButton.IsVisible = true;
            maxWindowMaxButton.IsVisible = false;
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

    private void Window_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)
    {
        int screenWidth = Screens.Primary.WorkingArea.Width;
        int screenHeight = Screens.Primary.WorkingArea.Height;

        if (this.Width > screenWidth && this.Height > screenHeight)
        {
            this.WindowState = WindowState.Maximized;
            normWindowMaxButton.IsVisible = false;
            maxWindowMaxButton.IsVisible = true;
        }

        if (this.Width < screenWidth && this.Height < screenHeight)
        {
            normWindowMaxButton.IsVisible = true;
            maxWindowMaxButton.IsVisible = false;
        }
    }

    private async void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        while (true)
        {
            await Task.Run(async () => await StartBackgroundProcess());
        }
    }

    private async Task StartBackgroundProcess()
    {
        await Dispatcher.UIThread.InvokeAsync(() => BackgroundProcesses());
    }

    private void BackgroundProcesses()
    {
        
    }
}

