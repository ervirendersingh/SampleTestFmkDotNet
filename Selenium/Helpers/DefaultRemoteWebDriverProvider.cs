using Ninject.Activation;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Selenium.AutomationToolInterfaces;
using System;
using System.Collections.Generic;


namespace Selenium.Helpers
{
    public class DefaultRemoteWebDriverProvider : Provider<IBrowserDriver>
    {
        private Dictionary<string, string> _browserSetttings = new Dictionary<string, string>();
        private string _hub;
        DesiredCapabilities capabilities;
        string _browserName;
        ChromeOptions options;

        public DefaultRemoteWebDriverProvider(string hub, Dictionary<string, string> browserSetttings)
        {
            _hub = hub;
            _browserSetttings = browserSetttings;
            _browserName = _browserSetttings["browserName"];
            capabilities = GetCapabilitiesFor(_browserSetttings["browserName"]);
            foreach (var settingKeyValuePair in _browserSetttings)
            {
                capabilities.SetCapability(settingKeyValuePair.Key, settingKeyValuePair.Value);
            }

            if(_browserName == "chrome")
            {
                options = new ChromeOptions();
                options.AddArguments("start-maximized");
                options.AddAdditionalCapability(CapabilityType.Platform, "LINUX", true);
                options.AddAdditionalCapability(CapabilityType.Version, "75.0.3770.80", true);
               // capabilities = options.ToCapabilities() as DesiredCapabilities;
              //  foreach (var settingKeyValuePair in _browserSetttings)
              //  {
              //      capabilities.SetCapability(settingKeyValuePair.Key, settingKeyValuePair.Value);
              //  }
            }
        }

        protected override IBrowserDriver CreateInstance(IContext context)
        {
            if (_hub != "")
            {
                return new BrowserDriver(_hub, options);
            }
            else
            {
                return new BrowserDriver(_browserSetttings["browserName"]);
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

     
    }
}
