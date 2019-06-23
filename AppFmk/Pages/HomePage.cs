
using AppFmk.Interfaces;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;

namespace AppFmk.Pages
{
    public class HomePage: ParentPage
    {
        private readonly ITestObject _login;

        public HomePage(IBasePage basePage) : base(basePage)
        {
            _login = TestObjectFactory.Get(Locator.CssSelector, "button.menu-tool:nth-child(2)");
        }

        public bool IsDisplayed()
        {
            return _login.IsDisplayed();
        }

        public void ClickLogin()
        {
            _login.Click();
        }
    }

    
}
