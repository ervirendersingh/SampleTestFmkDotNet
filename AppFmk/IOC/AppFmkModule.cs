using Ninject.Modules;
using AppFmk.Interfaces;
using AppFmk.Pages;
using AppFmk.ApplicationBasedHelpers;

using AppFmk.Reporting;
using Selenium.AutomationToolInterfaces;
using Selenium.Helpers;

namespace AppFmk.IOC
{
    public class AppFmkModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBasePage>().To<BasePage>();
            Bind<IFunctionExecuter>().To<Executer>();
            Bind<IReporter>().To<Reporter>();
            Bind<IResultPrinter>().To<ResultPrinter>();
            Bind<IVerificationInfo>().To<VerificationInfo>();
            Bind<IDependencyResolver>().To<DependencyResolver>();
            Bind<IVerification>().To<Verification>();

            //All Main Pages under Base Page
            Bind<HomePage>().ToSelf();
            Bind<LoginPage>().ToSelf();
         
           

        }
    }
}
