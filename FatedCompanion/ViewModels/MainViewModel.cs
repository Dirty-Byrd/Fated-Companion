using System;

namespace Fated_Companion.ViewModels;

public class MainViewModel : ViewModelBase
{
    public bool Windows => OperatingSystem.IsWindows();
    public bool Android => OperatingSystem.IsAndroid();


    public bool maximized = false;
    public bool normal = true;
}
