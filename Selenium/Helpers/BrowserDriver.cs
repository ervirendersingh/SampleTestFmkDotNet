using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Selenium.AutomationToolInterfaces;
using System;

namespace Selenium.Helpers
{
    public class BrowserDriver : IBrowserDriver
    {
        private IWebDriver _driver;

        public BrowserDriver(string _appium_hub, ChromeOptions options)
        {
            Console.Out.WriteLine("Trying to connect - " + _appium_hub);
            _driver = new RemoteWebDriver(new Uri(_appium_hub), options);
        }

        public BrowserDriver(string _appium_hub, DesiredCapabilities capabilities)
        {
            _driver = new RemoteWebDriver(new Uri(_appium_hub), capabilities, new TimeSpan(0, 3, 30));
        }

        public BrowserDriver(string browserName)
        {
            switch (browserName)
            {
                case "internet explorer":
                    _driver = new InternetExplorerDriver();
                    break;

                case "chrome":
                    _driver = new ChromeDriver();
                    break;

                case "firefox":
                    _driver = new FirefoxDriver();
                    break;

                case "MicrosoftEdge":
                    _driver = new EdgeDriver();
                    break;

                default:
                    _driver = new InternetExplorerDriver();
                    break;
            }
        }

        public IWebDriver GetDriver<IWebDriver>()
        {
            return (IWebDriver)_driver;
        }

        public void Maximize()
        {
            _driver.Manage().Window.Maximize();
        }
    }
}
