using Ninject.Modules;
using AppFmk.Interfaces;
using AppFmk.Pages;
using AppFmk.ApplicationBasedHelpers;
using AppFmk.Reporting;
using AppFmk.IOC;

namespace DependenciesResolver.IOC
{
    public class FmkModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBasePage>().To<BasePage>();
            Bind<IFunctionExecuter>().To<Executer>();
            Bind<IReporter>().To<Reporter>();
            Bind<IResultPrinter>().To<ResultPrinter>();
            Bind<IDependencyResolver>().To<DependencyResolver>();
               
        }
    }
}
