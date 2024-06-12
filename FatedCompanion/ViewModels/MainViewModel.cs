using System;
using Avalonia.Media;

namespace Fated_Companion.ViewModels;

public class MainViewModel : ViewModelBase
{
    //Determining OS
    public bool Windows => OperatingSystem.IsWindows();
    public bool Android => OperatingSystem.IsAndroid();
    
    //Informing Window State
    public bool Maximized = false;
    public bool Normal = true;
}

