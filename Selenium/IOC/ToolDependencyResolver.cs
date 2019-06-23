
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;
using Ninject.Parameters;
using OpenQA.Selenium;

namespace Selenium.IOC
{
    public class ToolDependencyResolver : IToolDependencyResolver
    {
        private readonly BrowserName browserName;
        public ToolDependencyResolver(BrowserName browserName)
        {
            this.browserName = browserName;
        }

        public T Resolve<T>()
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>();
        }

        public T Resolve<T>(IBrowser browser)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("browser", browser));
        }

        public T Resolve<T>(Locator locator, string value)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("locator", locator), 
                                 new ConstructorArgument("value", value));
        }

        public T Resolve<T>(IWebDriver driver, By locator)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("driver", driver), 
                                 new ConstructorArgument("locator", locator));
        }

        public T Resolve<T>(IBrowser browser, Locator locator, string value)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("browser", browser), 
                                 new ConstructorArgument("locator", locator), 
                                 new ConstructorArgument("value", value));
        }

        public T Resolve<T>(IBrowser browser, ITestObjectLocator toLocator)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("browser", browser),
                                 new ConstructorArgument("toLocator", toLocator));
        }
    }
}
