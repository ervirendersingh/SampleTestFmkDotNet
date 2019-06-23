

using OpenQA.Selenium;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;

namespace Selenium.IOC
{
    public interface IToolDependencyResolver
    {
        T Resolve<T>();
        T Resolve<T>(IBrowser browser);
        T Resolve<T>(Locator locator, string value);
        T Resolve<T>(IWebDriver driver, By locator);
        T Resolve<T>(IBrowser browser, Locator locator, string value);
        T Resolve<T>(IBrowser browser, ITestObjectLocator toLocator);
    }
}
