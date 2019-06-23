using AppFmk.Interfaces;
using Selenium.AutomationToolInterfaces;


namespace AppFmk.IOC
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
        T Resolve<T>(IBasePage basePage);
        T Resolve<T>(IBrowser browser);
        T Resolve<T>(IVerification toVerification);
    }
}
