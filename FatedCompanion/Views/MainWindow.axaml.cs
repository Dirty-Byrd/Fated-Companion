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

        //PopulateRulesetTree(@"Documents\Ruleset",RulesetTree);
        LoadDocumentViewer();
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
        if (DefaultTheme.IsChecked ?? true) 
        {
            DarkMode.IsEnabled = false;
        }

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

    private void DefaultTheme_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (DefaultTheme.IsChecked ?? true)
        {
            RequestedThemeVariant = ThemeVariant.Default;
            DarkMode.IsEnabled = false;
        }
        else
        {  
            DarkMode.IsEnabled = true;

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

    private void RulesetTree_SizeChanged(object? sender, SizeChangedEventArgs e)
    {

    }

    private void RulesetTree_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

    }

    private void PopulateRulesetTree(string DirectoryPath, object parent)
    {
        DirectoryPath = System.IO.Path.GetFullPath(DirectoryPath).Replace(@"bin\Debug\net7.0\", "");
        string[] folders = System.IO.Directory.GetDirectories(DirectoryPath);
        string[] files = System.IO.Directory.GetFiles(DirectoryPath);

        foreach (string file in files) // Iterates over files in subfolder
            {
                string fileName = System.IO.Path.GetFileName(file).Replace(".xps", ""); // Changes partial path into useable full path

                TreeViewItem childItem = new TreeViewItem(); // Creates new treeview item
                childItem.Header = fileName; // Sets the new treeview item's header to the name of the file

                if (parent.GetType() == typeof(TreeView))
                    {
                        TreeView parentItem = (TreeView)parent;
                        parentItem.Items.Add(childItem); // Adds treeview items as a child of folder treeview item
                    } 
                    else if (parent.GetType() == typeof(TreeViewItem))
                    {
                        TreeViewItem parentItem = (TreeViewItem)parent;
                        parentItem.Items.Add(childItem); // Adds treeview items as a child of folder treeview item
                    }
            }
            foreach (string folder in folders) // Iterates over array of folders
                {
                    string folderName = System.IO.Path.GetFileName(folder); // Variable that stores the name of the file, excluding path

                    TreeViewItem childDir = new TreeViewItem(); // Creates a new treeview item
                    childDir.Header = folderName; // Sets the new treeview item's header to the name of the file

                    if (parent.GetType() == typeof(TreeView))
                        {
                            TreeView parentItem = (TreeView)parent;
                            parentItem.Items.Add(childDir); // Adds treeview items as a child of folder treeview item
                        }
                        else if (parent.GetType() == typeof(TreeViewItem))
                        {
                            TreeViewItem parentItem = (TreeViewItem)parent;
                            parentItem.Items.Add(childDir); // Adds treeview items as a child of folder treeview item
                        }

                    PopulateRulesetTree(folder, childDir); // iterates the method to run on more subfolders
                }
    }

    private void LoadDocumentViewer()
    {
        string path = System.IO.Path.GetFullPath(@"Documents\Ruleset.mht").Replace(@".Desktop\bin\Debug\net7.0", @"\Assets");
        TreeViewItem testItem = new TreeViewItem();
        testItem.Header = path;
        RulesetTree.Items.Add(testItem);
        Uri rulesetWelcomeURL = new Uri(path);
        DocumentViewer.Url = rulesetWelcomeURL;
    }
}

