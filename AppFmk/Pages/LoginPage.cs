
using AppFmk.Interfaces;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;

namespace AppFmk.Pages
{
    public class LoginPage: ParentPage
    {
        private readonly ITestObject _goToLoginPortal;

        public LoginPage(IBasePage basePage) : base(basePage)
        {
            _goToLoginPortal = TestObjectFactory.Get(Locator.CssSelector, "body > header > div > div > div.row > div.menu-tools.desktop > button:nth-child(2)");
        }

        public bool IsDisplayed()
        {
            return _goToLoginPortal.IsDisplayed();
        }

        public void ClickToNavigateLoginPortal()
        {
            _goToLoginPortal.Click();
        }
    }

    
}
