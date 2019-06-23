
using AppFmk.Pages;

using Selenium.AutomationToolInterfaces;
using AppFmk.IOC;
using Selenium.IOC;
using Selenium.Enums;

namespace AppFmk.Interfaces
{
    public interface IBasePage
    {
        HomePage HomePage { get; }
        LoginPage LoginPage { get; }
        string RunningBrowser { get; set; }
        IBrowser GetBrowser();
        IToolDependencyResolver GetToolDependencyResolver();
        IDependencyResolver GetResolver();
        string TestName { get; set; }
        string FixtureName { get; set; }
        string ScreenShotsPath { get; set; }
        void ShouldQuitTheBrowser();
        void ShouldTakeMeToHomePage();
        void ShouldTakeMeToAddressAt(string url);
        void SwitchToAnotherWindow();
        void CloseAllOtherWindows();
        string SaveScreenshot();
        void Dispose();
        void MaximizeBrowser();
    }
}
