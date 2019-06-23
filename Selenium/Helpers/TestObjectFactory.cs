using Selenium.AutomationToolInterfaces;
using Selenium.IOC;
using Selenium.Enums;


namespace Selenium.Helpers
{
    public class TestObjectFactory : ITestObjectFactory
    {
        private IBrowser _browser { get; set; }
        IToolDependencyResolver Resolver;
        ITestObjectLocator TOLocator;

        public TestObjectFactory(IBrowser browser, IToolDependencyResolver resolver)
        {
            _browser = browser;
            Resolver = resolver; 
        }

        public ITestObject Get(Locator locator, string value)
        {
            TOLocator = Resolver.Resolve<ITestObjectLocator>(locator, value);
            return Get(TOLocator);
        }

        public ITestObject Get(ITestObjectLocator toLocator)
        {
            return Resolver.Resolve<ITestObject>(_browser, toLocator);
        }
    }
}
