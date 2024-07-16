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
using System.IO;

namespace Fated_Companion.Views;

public class Settings
{
    bool DefaultThemeSwitchOn;
    bool DarkModeSwitchOn;
}

public partial class MainWindow : Window
{
    string selectedTreeViewItemPath = @"Assets\Documents\Ruleset.mht";
    string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Fated");
    string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Fated", "Settings","fated.set");

    Settings AppSettings = new Settings();

    public MainWindow()
    {
        CreateDirectories();

        LoadSettings();
        GetSettings();
        SaveSettings();

        InitializeComponent();

        NormWindowMaxButton.IsVisible = true;
        MaxWindowMaxButton.IsVisible = false;

        PopulateRulesetTree(@"Assets\Documents\Ruleset",RulesetTree);

        StartupDocumentsThemes();
    }

    private void CreateDirectories()
    {
        Directory.CreateDirectory(AppDataPath);
        Directory.CreateDirectory(SettingsPath);
    }

    private void LoadSettings()
    {
        try
        {
            using (FileStream fs = new FileStream(SettingsPath, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Settings));

                AppSettings = x.Deserialize(fs) as Settings;
            }
        } 
        catch
        {

        }
    }

    private void GetSettings()
    {

    }

    private void SaveSettings()
    {
        using (StreamWriter writer = new StreamWriter(SettingsPath))
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            x.Serialize(writer, AppSettings);
        }
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
        if (DefaultThemeSwitch.IsChecked ?? true) 
        {
            DarkModeSwitch.IsEnabled = false;
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
        Home.IsVisible = true;
        Ruleset.IsVisible = false;
        Tapestries.IsVisible = false;
        Tools.IsVisible = false;
        Settings.IsVisible = false;
    }

    private void RulesetButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Home.IsVisible = false;
        Ruleset.IsVisible = true;
        Tapestries.IsVisible = false;
        Tools.IsVisible = false;
        Settings.IsVisible = false;
    }

    private void TapestriesButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Home.IsVisible = false;
        Ruleset.IsVisible = false;
        Tapestries.IsVisible = true;
        Tools.IsVisible = false;
        Settings.IsVisible = false;
    }

    private void ToolsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Home.IsVisible = false;
        Ruleset.IsVisible = false;
        Tapestries.IsVisible = false;
        Tools.IsVisible = true;
        Settings.IsVisible = false;
    }
    private void SettingsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Home.IsVisible = false;
        Ruleset.IsVisible = false;
        Tapestries.IsVisible = false;
        Tools.IsVisible = false;
        Settings.IsVisible = true;
    }

    private void DarkMode_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (DarkModeSwitch.IsChecked ?? true)
        {
            RequestedThemeVariant = ThemeVariant.Dark;
            ChangeDocumentsThemes("Dark", @"Assets\Documents");
        }
        else
        {
            RequestedThemeVariant = ThemeVariant.Light;
            ChangeDocumentsThemes("Light", @"Assets\Documents");
        }

        DocumentViewer.Reload();
    }

    private void DefaultTheme_IsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (DefaultThemeSwitch.IsChecked ?? true)
        {
            RequestedThemeVariant = ThemeVariant.Default;
            DarkModeSwitch.IsEnabled = false;
            
            ThemeVariant currentTheme = ActualThemeVariant;

            if (currentTheme == ThemeVariant.Light)
            {
                ChangeDocumentsThemes("Light", @"Assets\Documents");
            }
            else if (currentTheme == ThemeVariant.Dark)
            {
                ChangeDocumentsThemes("Dark", @"Assets\Documents");
            }
        }
        else
        {  
            DarkModeSwitch.IsEnabled = true;

            if (DarkModeSwitch.IsChecked ?? true)
            {
                RequestedThemeVariant = ThemeVariant.Dark;
                ChangeDocumentsThemes("Dark", @"Assets\Documents");
            }
            else
            {
                RequestedThemeVariant = ThemeVariant.Light;
                ChangeDocumentsThemes("Light", @"Assets\Documents");
            }
        }

        DocumentViewer.Reload();
    }

    private void RulesetTree_SizeChanged(object? sender, SizeChangedEventArgs e)
    {

    }

    private void RulesetTree_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        TreeViewItem? item = RulesetTree.SelectedItem as TreeViewItem;

        bool loop = true;

        string path = item.Header.ToString() + ".mht";

        while (loop)
        {
            TreeViewItem? parent = item.Parent as TreeViewItem;

            try
            {
                path = parent.Header.ToString() + "\\" + path;
            }
            catch
            {
                loop = false; 
                break;
            }
            item = parent as TreeViewItem;
        }
        selectedTreeViewItemPath = "Assets\\Documents\\Ruleset\\" + path;

        try
        {
            LoadDocumentViewer(selectedTreeViewItemPath);
        }
        catch
        {

        }
    }

    private void PopulateRulesetTree(string DirectoryPath, object parent)
    {
        DirectoryPath = System.IO.Path.GetFullPath(DirectoryPath).Replace(@".Desktop\bin\Debug\net7.0\", @"\").Replace(@".Desktop\bin\Release\net7.0\win-x86\", @"\");
        string[] folders = System.IO.Directory.GetDirectories(DirectoryPath);
        string[] files = System.IO.Directory.GetFiles(DirectoryPath);

        foreach (string file in files) // Iterates over files in subfolder
            {
                string fileName = System.IO.Path.GetFileName(file).Replace(".mht", ""); // Changes partial path into useable full path

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

    private void ChangeDocumentsThemes(string Theme, string Path)
    {
        string DocumentsPath = System.IO.Path.GetFullPath(Path).Replace(@".Desktop\bin\Debug\net7.0\", @"\").Replace(@".Desktop\bin\Release\net7.0\win-x86\", @"\");
        string[] folders = System.IO.Directory.GetDirectories(DocumentsPath);
        string[] files = System.IO.Directory.GetFiles(DocumentsPath);



            foreach (string file in files) // Iterates over files in subfolder
            {
                if (Theme == "Dark")
                {                    
                    string fileContents = File.ReadAllText(file);
                    string newText = fileContents.Replace("#1e1e1e", "white").Replace("WHITE", "#1E1E1E");
                    File.WriteAllText(file, newText);
                    LoadDocumentViewer(selectedTreeViewItemPath);

            } else
                {
                    string fileContents = File.ReadAllText(file);
                    string newText = fileContents.Replace("#1E1E1E", "WHITE").Replace("white", "#1e1e1e");
                    File.WriteAllText(file, newText);
                    LoadDocumentViewer(selectedTreeViewItemPath);
                }
            }
            foreach (string folder in folders) // Iterates over array of folders
            {
                string folderName = System.IO.Path.GetFileName(folder); // Variable that stores the name of the file, excluding path


                ChangeDocumentsThemes(Theme,folder); // iterates the method to run on more subfolders
            }
    }

    private void StartupDocumentsThemes()
    {
        ThemeVariant currentTheme = ActualThemeVariant;

        if (currentTheme == ThemeVariant.Light)
        {
            ChangeDocumentsThemes("Light", @"Assets\Documents");
        }
        else if (currentTheme == ThemeVariant.Dark)
        {
            ChangeDocumentsThemes("Dark", @"Assets\Documents");
        }
    }

    private void WebView_Loaded(object sender, RoutedEventArgs e)
    {
        LoadDocumentViewer(@"Assets\Documents\Ruleset.mht");
    }

    private void LoadDocumentViewer(string Path)
    {
        string path = System.IO.Path.GetFullPath(Path).Replace(@".Desktop\bin\Debug\net7.0\", @"\").Replace(@".Desktop\bin\Release\net7.0\win-x86\", @"\");
        
        if (File.Exists(path))
        {
            

            Uri documentURL = new Uri(path);
            DocumentViewer.Url = documentURL;
        }
    }
}

