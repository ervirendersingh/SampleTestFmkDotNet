using System.Configuration;
using Selenium.AutomationToolInterfaces;
using AppFmk.Interfaces;
using AppFmk.IOC;
using Selenium.IOC;
using Selenium.Enums;
using System;

namespace AppFmk.Pages
{
    public class BasePage : IBasePage
    {
        private IBrowser _browser;
        private IScreenshotTaker _screenShotTaker;
        private IDependencyResolver _resolver;
        private IToolDependencyResolver _toolDependencyResolver;

        public HomePage HomePage { get; set; }
        public LoginPage LoginPage { get; set; }
        
        public string TestName { get; set; }
        public string FixtureName { get; set; }
        public string ScreenShotsPath { get; set; }
        public string RunningBrowser { get; set; }

        public BasePage(IBrowser browser, IDependencyResolver resolver, 
                        IToolDependencyResolver toolDependencyResolver)
        {
            _browser = browser;
            _resolver = resolver;
            _toolDependencyResolver = toolDependencyResolver;
            _screenShotTaker = _toolDependencyResolver.Resolve<IScreenshotTaker>(this.GetBrowser());
            HomePage = _resolver.Resolve<HomePage>(this);
            LoginPage = _resolver.Resolve<LoginPage>(this);
        }

        public IBrowser GetBrowser()
        {
            return _browser;
        }

        public IToolDependencyResolver GetToolDependencyResolver()
        {
            return _toolDependencyResolver;
        }

        public IDependencyResolver GetResolver()
        {
            return _resolver;
        }

        public void ShouldQuitTheBrowser()
        {
            _browser.Quit();
        }

        public void Dispose()
        {
            _browser.Dispose();
        }

        public void MaximizeBrowser()
        {
            _browser.Maximize();
        }


        public void ShouldTakeMeToHomePage()
        {
            _browser.Navigate(ConfigurationManager.AppSettings.Get("AppURI"));
        }

        public void ShouldTakeMeToAddressAt(string url)
        {
            _browser.Navigate(url);
        }

        public void SwitchToAnotherWindow()
        {
            _browser.SwitchToOtherWindow();
        }

        public void CloseAllOtherWindows()
        {
            _browser.CloseAllOtherWindows();
        }

        public string SaveScreenshot()
        {
            string testCode = "";
            if (TestName.Contains("_"))
                testCode = TestName.Split('_')[1];
            else
                testCode = TestName.Split('(')[0];
            string filePath = ScreenShotsPath + "\\" + testCode;
            _screenShotTaker.SaveScreenShotAt(filePath);
            return testCode + ".jpeg";
        }
    }
}
