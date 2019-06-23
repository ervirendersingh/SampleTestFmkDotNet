using Ninject.Activation;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using Selenium.AutomationToolInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Helpers
{
    public class SeleniumSafariDriverProvider : Provider<IBrowserDriver>
    {
        private Dictionary<string, string> _browserSetttings = new Dictionary<string, string>();
        private string _appium;
        private string _hub;
        DesiredCapabilities capabilities;

        public SeleniumSafariDriverProvider(string hub, string appium, Dictionary<string, string> browserSetttings)
        {
            capabilities = new DesiredCapabilities();
            _hub = hub;
            _appium = appium;
            _browserSetttings = browserSetttings;
            _browserSetttings["browserName"] = "safari";
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
