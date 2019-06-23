using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using Selenium.AutomationToolInterfaces;

namespace Selenium.Helpers
{
    public class Browser : IBrowser
    {
        private IBrowserDriver _instance;
        private CustomJavaScriptExecuter JSExecuter;

        public Browser(IBrowserDriver browserDriver)
        {
            _instance = browserDriver;
        }

        public IBrowserDriver GetInstance()
        {
            return _instance;
        }

        public void Navigate(string url)
        {
            _instance.GetDriver<IWebDriver>().Navigate().GoToUrl(url);
            WaitForPageToFinishLoading();
        }

        public void Quit()
        {
            _instance.GetDriver<IWebDriver>().Quit();
        }

        public void Dispose()
        {
            _instance.GetDriver<IWebDriver>().Dispose();
        }

        public void Close()
        {
            _instance.GetDriver<IWebDriver>().Close();
        }

        public string CurrentWindowHandle { get { return _instance.GetDriver<IWebDriver>().CurrentWindowHandle; } }
        public string PageSource { get { return _instance.GetDriver<IWebDriver>().PageSource; } }
        public string Title { get { return _instance.GetDriver<IWebDriver>().Title; } }
        public string Url { get { return _instance.GetDriver<IWebDriver>().Url; } }
        public IList<string> WindowHandles { get { return _instance.GetDriver<IWebDriver>().WindowHandles; } }

        public void Maximize()
        {
            _instance.GetDriver<IWebDriver>().Manage().Window.Maximize();
        }


        private void WaitForAlertToPresent()
        {
            try
            {
                IWait<IWebDriver> wait = new WebDriverWait(_instance.GetDriver<IWebDriver>(), System.TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.AlertIsPresent());
            }
            catch (Exception e)
            {
            }
        }

        public void SwitchToAlert()
        {
            WaitForAlertToPresent();
            _instance.GetDriver<IWebDriver>().SwitchTo().Alert();
        }

        public void AcceptAlert()
        {
            if (IsAlertPresent())
            {
                IAlert alert = _instance.GetDriver<IWebDriver>().SwitchTo().Alert();
                alert.Accept();
            }
        }

        public void DismissAlert()
        {
            if (IsAlertPresent())
            {
                IAlert alert = _instance.GetDriver<IWebDriver>().SwitchTo().Alert();
                alert.Dismiss();
            }
        }

        public string GetAlertText()
        {
            if (IsAlertPresent())
            {
                IAlert alert = _instance.GetDriver<IWebDriver>().SwitchTo().Alert();
                return alert.Text;
            }
            else
                return "";
        }

        private bool IsAlertPresent()
        {
            WaitForAlertToPresent();
            try
            {
                _instance.GetDriver<IWebDriver>().SwitchTo().Alert();
            }
            catch (NoAlertPresentException e)
            {
                return false;
            }

            return true;
        }

        public void SwitchToFrame(int frameIndex)
        {
            _instance.GetDriver<IWebDriver>().SwitchTo().Frame(frameIndex);
        }

        public void SwitchToWindow(string windowName)
        {
            _instance.GetDriver<IWebDriver>().SwitchTo().Window(windowName);
        }

        public void SwitchToOtherWindow()
        {
            string curWindowHandle = _instance.GetDriver<IWebDriver>().CurrentWindowHandle;
            foreach (var handle in _instance.GetDriver<IWebDriver>().WindowHandles)
            {
                if (handle != curWindowHandle)
                {
                    _instance.GetDriver<IWebDriver>().SwitchTo().Window(handle);
                    Focus();
                }
            }
        }

        private bool isPageLoaded(IWebDriver driver)
        {
            string script = "if(document.readyState == 'complete') return 'true';";
            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            //string pageLoaded = js.ExecuteScript(script).ToString();
            JSExecuter = new CustomJavaScriptExecuter(driver);
            string pageLoaded = JSExecuter.Execute(script, new object[] { });

            //Retry again if problem exists
            if (pageLoaded == "" || pageLoaded == "customException")
            {
                System.Console.WriteLine("Script Tried Again in isPageLoaded");
                pageLoaded = JSExecuter.Execute(script, new object[] { });
            }

            if (pageLoaded == "true")
                return true;
            else
                return false;
        }


        public void WaitForPageToFinishLoading()
        {
            WebDriverWait wait = new WebDriverWait(_instance.GetDriver<IWebDriver>(), System.TimeSpan.FromSeconds(15));

            try
            {
                wait.Until(wd => isPageLoaded(wd));
            }
            catch (WebDriverTimeoutException)
            {
            }
        }

        public void Focus()
        {
            string script = "window.focus();";
            script += "return '';";

            IJavaScriptExecutor js = (IJavaScriptExecutor)_instance.GetDriver<IWebDriver>();
            js.ExecuteScript(script);
        }

        public void CloseAllOtherWindows()
        {
            string curWindowHandle = _instance.GetDriver<IWebDriver>().CurrentWindowHandle;

            foreach (var handle in _instance.GetDriver<IWebDriver>().WindowHandles)
            {
                if (handle != curWindowHandle)
                {
                    _instance.GetDriver<IWebDriver>().SwitchTo().Window(handle);
                    _instance.GetDriver<IWebDriver>().Close();
                }
            }

            _instance.GetDriver<IWebDriver>().SwitchTo().Window(curWindowHandle);
        }

       

        /*
        public void InitializeBrowser(string hub, string appium, Dictionary<string, string> browserSettings)
        {
            DesiredCapabilities capabilities = GetCapabilitiesFor(browserSettings["browserName"]);


            switch (browserSettings["browserName"])
            {
                case "safariIphone":
                    {
                        browserSettings["browserName"] = "safari";
                        foreach (var settingKeyValuePair in browserSettings)
                        {
                            capabilities.SetCapability(settingKeyValuePair.Key, settingKeyValuePair.Value);
                        }
                        _instance = new RemoteWebDriver(new Uri(appium), capabilities, new TimeSpan(0, 3, 30));
                        break;
                    }

                case "androidchrome":
                    {
                        browserSettings["browserName"] = "chrome";
                        foreach (var settingKeyValuePair in browserSettings)
                        {
                            capabilities.SetCapability(settingKeyValuePair.Key, settingKeyValuePair.Value);
                        }
                        _instance = new RemoteWebDriver(new Uri(appium), capabilities, new TimeSpan(0, 3, 30));
                        break;
                    }

                default:
                    {
                        foreach (var settingKeyValuePair in browserSettings)
                        {
                            capabilities.SetCapability(settingKeyValuePair.Key, settingKeyValuePair.Value);
                        }
                        if (hub != "")
                        {
                            _instance = new RemoteWebDriver(new Uri(hub), capabilities, new TimeSpan(0, 3, 30));
                        }
                        else
                        {
                            InitializeBrowser(browserSettings["browserName"]);
                        }
                        Maximize();
                        break;
                    }
            }
        }

            
        private DesiredCapabilities GetCapabilitiesFor(string browserName)
        {
            switch (browserName)
            {
                case "internet explorer":
                    return DesiredCapabilities.InternetExplorer();

                case "chrome":
                    return DesiredCapabilities.Chrome();

                case "firefox":
                    return DesiredCapabilities.Firefox();

                case "MicrosoftEdge":
                    return DesiredCapabilities.Edge();

                case "androidchrome":
                    return new DesiredCapabilities();

                case "safari":
                    return DesiredCapabilities.Safari();

                case "safariIphone7":
                    return new DesiredCapabilities();

                default:
                    return null;
            }
        }

        public void InitializeBrowser(string browserName)
        {
            
            switch (browserName)
            {
                case "internet explorer":
                    _instance = new InternetExplorerDriver();
                    break;

                case "chrome":
                    _instance = new ChromeDriver();
                    break;

                case "firefox":
                    _instance = new FirefoxDriver();
                    break;

                case "MicrosoftEdge":
                    _instance = new EdgeDriver();
                    break;

                default:
                    _instance = new InternetExplorerDriver();
                    break;
            }
        }
        */


    }
}
