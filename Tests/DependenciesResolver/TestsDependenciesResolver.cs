
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using Ninject.Parameters;
using System.Collections.Specialized;
using AppFmk.Interfaces;
using AppFmk.IOC;
using DependenciesResolver.IOC;

namespace Tests.DependenciesResolver
{
    public class TestsDependenciesResolver : ITestsDependenciesResolver
    {
        string HUB;
        string APPIUM;
        string RUNALLTESTSON;
        NameValueCollection BROWSERSETTINGS;

        public TestsDependenciesResolver()
        {
        }

        public TestsDependenciesResolver(string hub, string appium, string runAllTestsOn, 
                                        NameValueCollection browserSettingsKeyValuePairColl)
        {
            HUB = hub;
            APPIUM = appium;
            RUNALLTESTSON = runAllTestsOn;
            BROWSERSETTINGS = browserSettingsKeyValuePairColl;
        }

        public T Resolve<T>()
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new FmkModule(),
                new SeleniumInternalsModule(HUB,
                                            APPIUM,
                                            RUNALLTESTSON,
                                            BROWSERSETTINGS)
                
            };
            kernel.Load(modules);

            return kernel.Get<T>();
        }

        public T Resolve<T>(IBasePage basePage)
        {
            IKernel kernel = new StandardKernel();
            var modules = new List<INinjectModule>
            {
                new FmkModule(),
                new SeleniumInternalsModule()

            };
            kernel.Load(modules);

            return kernel.Get<T>(new ConstructorArgument("basePage", basePage));
        }      
    }
}
