using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Styling;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Data;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Threading;
using Fated_Companion.ViewModels;

namespace Fated_Companion.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NormWindowMaxButton.IsVisible = true;
        MaxWindowMaxButton.IsVisible = false;
        
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
            NormWindowMaxButton.IsVisible = false;
            MaxWindowMaxButton.IsVisible = true;
            CloseButton.CornerRadius = new CornerRadius(0);
            MainBorder.CornerRadius = new CornerRadius(0);
        }
        else if (this.WindowState == WindowState.Maximized)
        {
            this.WindowState = WindowState.Normal;
            NormWindowMaxButton.IsVisible = true;
            MaxWindowMaxButton.IsVisible = false;
            CloseButton.CornerRadius = new CornerRadius(0,8,0,0);
            MainBorder.CornerRadius = new CornerRadius(0,8,0,0);
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
            NormWindowMaxButton.IsVisible = false;
            MaxWindowMaxButton.IsVisible = true;
            CloseButton.CornerRadius = new CornerRadius(0);
            MainBorder.CornerRadius = new CornerRadius(0);
        }

        if (this.Width < screenWidth && this.Height < screenHeight)
        {
            NormWindowMaxButton.IsVisible = true;
            MaxWindowMaxButton.IsVisible = false;
            CloseButton.CornerRadius = new CornerRadius(0,8,0,0);
            MainBorder.CornerRadius = new CornerRadius(0,8,0,0);
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
    
    private void HomeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.SelectedIndex = 0;
    }

    private void RulesetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.SelectedIndex = 1;
    }

    private void TapestriesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.SelectedIndex = 2;
    }

    private void ToolsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.SelectedIndex = 3;
    }
    private void SettingsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MainContent.SelectedIndex = 4;
    }

    private void DarkMode_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (DarkMode.IsChecked ?? true)
        {
            RequestedThemeVariant = ThemeVariant.Dark;
        }
        else
        {
            RequestedThemeVariant = ThemeVariant.Light;
        }
    }
}

