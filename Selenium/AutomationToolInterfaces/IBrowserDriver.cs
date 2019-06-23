using OpenQA.Selenium;


namespace Selenium.AutomationToolInterfaces
{
    public interface IBrowserDriver
    {
        T GetDriver<T>();
    }
}
