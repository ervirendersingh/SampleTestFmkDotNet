

using AppFmk.Interfaces;
using AppFmk.IOC;
using Selenium.AutomationToolInterfaces;
using Selenium.IOC;

namespace AppFmk.Pages
{
    public class ParentPage
    {
        public ITestObjectFactory TestObjectFactory;
        public IDependencyResolver Resolver;
        public IToolDependencyResolver ToolDependencyResolver;
        public IBasePage BasePage;
 

        public ParentPage(IBasePage basePage)
        {
            BasePage = basePage;
            ToolDependencyResolver = basePage.GetToolDependencyResolver();
            Resolver = basePage.GetResolver();
            
            TestObjectFactory = ToolDependencyResolver.Resolve<ITestObjectFactory>(basePage.GetBrowser());
        }
    }
}
