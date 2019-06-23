
using Ninject.Modules;
using Selenium.AndroidChrome;
using Selenium.AutomationToolInterfaces;
using Selenium.Edge;
using Selenium.Enums;
using Selenium.Firefox;
using Selenium.Helpers;
using Selenium.InternetExplorer;
using Selenium.Safari;

namespace Selenium.IOC
{
    public class SeleniumModule : NinjectModule
    {
        private readonly BrowserName browserSettingsToLoad = BrowserName.Chrome;
        public SeleniumModule()
        {
        }

        public SeleniumModule(BrowserName browserName)
        {
            this.browserSettingsToLoad = browserName;
        }

        public override void Load()
        {
            Bind<ITestObjectFactory>().To<TestObjectFactory>();
            Bind<IBrowser>().To<Browser>();
            Bind<IScreenshotTaker>().To<ScreenshotTaker>();

            switch(this.browserSettingsToLoad)
            {
                case BrowserName.Firefox:
                    {
                        Bind<ITestObject>().To<FirefoxTestObject>();
                        Bind<IWaitFor>().To<FirefoxWaitFor>();
                        break;
                    }

                case BrowserName.Safari:
                    {
                        Bind<ITestObject>().To<SafariTestObject>();
                        Bind<IWaitFor>().To<SafariWaitFor>();
                        break;
                    }

                case BrowserName.IE11:
                    {
                        Bind<ITestObject>().To<IETestObject>();
                        Bind<IWaitFor>().To<IEWaitFor>();
                        break;
                    }

                case BrowserName.AndroidChromeNexus6:
                    {
                        Bind<ITestObject>().To<AndroidChromeTestObject>();
                        Bind<IWaitFor>().To<AndroidChromeWaitFor>();
                        break;
                    }

                case BrowserName.Edge:
                    {
                        Bind<ITestObject>().To<EdgeTestObject>();
                        Bind<IWaitFor>().To<EdgeWaitFor>();
                        break;
                    }

                default:
                    {
                        Bind<ITestObject>().To<TestObject>();
                        Bind<IWaitFor>().To<WaitFor>();
                        break;
                    }

            }
            
            Bind<ITestObjectLocator>().To<TestObjectLocator>();
            Bind<IVerification>().To<Verification>();
            Bind<IToolDependencyResolver>().ToProvider(new ToolsDependencyResolverProvider(browserSettingsToLoad));
        }
        
    }
}
