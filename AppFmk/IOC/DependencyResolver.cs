
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using Ninject.Parameters;
using System.Collections.Specialized;
using AppFmk.Interfaces;
using Selenium.AutomationToolInterfaces;
using Selenium.IOC;
using Selenium.Enums;
using System.Configuration;
using System;

namespace AppFmk.IOC
{
    public class DependencyResolver : IDependencyResolver
    {
        //private readonly BrowserName browserName;

        public DependencyResolver()
        {
            //browserName = (BrowserName)Enum.Parse(typeof(BrowserName),ConfigurationManager.AppSettings.Get("RunAllTestsOn"));
        }

        public T Resolve<T>()
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new AppFmkModule(),
                //new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>();
        }

        public T Resolve<T>(BrowserName browserName)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new AppFmkModule(),
                //new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>();
        }

        public T Resolve<T>(IBasePage basePage)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new AppFmkModule(),
                //new SeleniumModule(browserName)
            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("basePage", basePage));
        }

        
        public T Resolve<T>(IBrowser browser)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new AppFmkModule(),
                //new SeleniumModule(browserName)

            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("browser", browser));
        }

        public T Resolve<T>(IVerification toVerification)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new AppFmkModule(),
               // new SeleniumModule(browserName)

            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("toVerification", toVerification));
        }
    }
}
