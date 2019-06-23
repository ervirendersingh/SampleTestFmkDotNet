
using Ninject.Modules;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;
using Selenium.Helpers;
using Selenium.IOC;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DependenciesResolver.IOC
{
    public class SeleniumInternalsModule : NinjectModule
    {
        string hub;
        string appium;
        Dictionary<string, string> browserSettings = new Dictionary<string, string>();
        BrowserName browserName;
        public SeleniumInternalsModule()
        {
        }

        public SeleniumInternalsModule(string hub, string appium, string runAllTestsOn, 
                                        NameValueCollection browserSettingsKeyValuePairColl)
        {
            this.hub = hub;
            this.appium = appium;
            this.browserName = (BrowserName)Enum.Parse(typeof(BrowserName), runAllTestsOn);
            this.browserSettings = GetBrowserConfigurationFor(runAllTestsOn, browserSettingsKeyValuePairColl);
        }


        private Dictionary<string, string> GetBrowserConfigurationFor(string settingsSectionName, 
                                                NameValueCollection browserSettingsKeyValuePairColl)
        {
            Dictionary<string, string>  settings = new Dictionary<string, string>();
            if (browserSettingsKeyValuePairColl != null)
            {
                foreach (var settingKey in browserSettingsKeyValuePairColl.AllKeys)
                {
                    var value = browserSettingsKeyValuePairColl.Get(settingKey);
                    if (value != null)
                    {
                        settings.Add(settingKey, value);
                    }
                }
            }
            return settings;
        }

        public override void Load()
        {
            Bind<IBrowser>().To<Browser>();
            Bind<IToolDependencyResolver>().ToProvider(new ToolsDependencyResolverProvider(browserName));

            if (browserSettings.Count > 0)
            {
                switch (browserSettings["browserName"])
                {
                    case "safariIphone":
                        {
                            Bind<IBrowserDriver>().ToProvider(new SeleniumSafariDriverProvider(hub, appium, browserSettings));
                            break;
                        }

                    case "androidchrome":
                        {
                            Bind<IBrowserDriver>().ToProvider(new AndroidChromeRemoteWebDriverProvider(hub, appium, browserSettings));
                            break;
                        }

                    default:
                        {
                            Bind<IBrowserDriver>().ToProvider(new DefaultRemoteWebDriverProvider(hub, browserSettings));
                            break;
                        }
                }
            }
        }
        
    }
}
