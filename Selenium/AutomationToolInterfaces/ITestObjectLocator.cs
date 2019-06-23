
using OpenQA.Selenium;
using Selenium.Enums;

namespace Selenium.AutomationToolInterfaces
{
    public interface ITestObjectLocator
    {
        Locator LocateBy { get; set; }
        string Value { get; set; }
        By GetLocator();
    }
}
