using OpenQA.Selenium;
using Selenium.AutomationToolInterfaces;
using Selenium.Enums;

namespace Selenium.Helpers
{
    public class TestObjectLocator : ITestObjectLocator
    {
        public Locator LocateBy { get; set; }
        public string Value { get; set; }

        public TestObjectLocator(Locator locator, string value)
        {
            LocateBy = locator;
            Value = value;
        }

        public By GetLocator()
        {
            switch (LocateBy)
            {
                case Locator.Id:
                    return By.Id(Value);

                case Locator.LinkText:
                    return By.LinkText(Value);

                case Locator.XPath:
                    return By.XPath(Value);

                case Locator.Name:
                    return By.Name(Value);

                case Locator.PartialLinkText:
                    return By.PartialLinkText(Value);

                case Locator.TagName:
                    return By.TagName(Value);

                case Locator.CssSelector:
                    return By.CssSelector(Value);

                case Locator.ClassName:
                    return By.ClassName(Value);

                default:
                    return null;
            }

        }

    }
}
