using Selenium.Enums;

namespace Selenium.AutomationToolInterfaces
{
    public interface ITestObjectFactory
    {
        ITestObject Get(Locator locator, string value);
        ITestObject Get(ITestObjectLocator locator);
    }
}
