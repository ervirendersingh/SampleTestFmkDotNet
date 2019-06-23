using Ninject.Activation;
using OpenQA.Selenium.Remote;
using Selenium.AutomationToolInterfaces;
using System;
using System.Collections.Generic;

namespace Selenium.Helpers
{
    public class AndroidChromeRemoteWebDriverProvider : Provider<IBrowserDriver>
    {
        private Dictionary<string, string> _browserSetttings = new Dictionary<string, string>();
        private string _appium;
        private string _hub;
        DesiredCapabilities capabilities;

        public AndroidChromeRemoteWebDriverProvider(string hub, string appium, Dictionary<string, string> browserSetttings)
        {
            capabilities = new DesiredCapabilities();
            _appium = appium;
            _hub = hub;
            _browserSetttings = browserSetttings;
            _browserSetttings["browserName"] = "chrome";
            foreach (var settingKeyValuePair in _browserSetttings)
            {
                capabilities.SetCapability(settingKeyValuePair.Key, settingKeyValuePair.Value);
            }
        }
        protected override IBrowserDriver CreateInstance(IContext context)
        {
            if (_hub != "")
                return new BrowserDriver(_hub, capabilities);
            else
                return new BrowserDriver(_appium, capabilities);
        }
    }
}
