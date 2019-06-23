
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
            _goToLoginPortal = TestObjectFactory.Get(Locator.CssSelector, "button.btn.btn-arrow.no-arrow.white.white-gradient.has-label");
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
