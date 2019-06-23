using System.Collections.Generic;

namespace Selenium.AutomationToolInterfaces
{
    public interface IBrowser
    {
        IBrowserDriver GetInstance();
        void Focus();
        void Navigate(string url);
        void Quit();
        void Close();
        void Maximize();
        void SwitchToAlert();
        void AcceptAlert();
        void DismissAlert();
        void SwitchToWindow(string windowName);
        void SwitchToOtherWindow();
        string CurrentWindowHandle { get; }
        string PageSource { get;  }
        string Title { get; }
        string Url { get; }
        IList<string> WindowHandles { get; }
        void WaitForPageToFinishLoading();
        string GetAlertText();
        void CloseAllOtherWindows();
        void Dispose();
    }
    
}
