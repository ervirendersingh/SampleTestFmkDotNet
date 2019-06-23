using BoDi;
using TechTalk.SpecFlow;
using AppFmk.Interfaces;
using System.Configuration;
using System.Collections.Specialized;
using Tests.DependenciesResolver; 

namespace Tests
{
   [Binding]
    public class TestStartUp
    {
        private readonly IObjectContainer _objectContainer;
        private IBasePage _basePage;
        private IFunctionExecuter _executer;
        private IReporter _reporter;

        public TestStartUp(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void SetUp()
        {
            string Hub = ConfigurationManager.AppSettings.Get("Hub");
            string Appium = ConfigurationManager.AppSettings.Get("Appium");
            string RunAllTestsOn = ConfigurationManager.AppSettings.Get("RunAllTestsOn");
            var sectionNamePath = "Browsers/" + ConfigurationManager.AppSettings.Get("RunAllTestsOn");
            var browserSettingsKeyValuePairColl = ConfigurationManager.GetSection(sectionNamePath) as NameValueCollection;

            ITestsDependenciesResolver resolver = new TestsDependenciesResolver(Hub, Appium, RunAllTestsOn, 
                                                                                browserSettingsKeyValuePairColl);
            _basePage = resolver.Resolve<IBasePage>();
            _executer = resolver.Resolve<IFunctionExecuter>(_basePage);
            _reporter = resolver.Resolve<IReporter>(_basePage);

            _objectContainer.RegisterInstanceAs(_basePage);
            _objectContainer.RegisterInstanceAs(_executer);
            _objectContainer.RegisterInstanceAs(_reporter);
        }

        [AfterScenario]
        public void TearDown()
        {
            try
            {
                _basePage.ShouldQuitTheBrowser();
            }
            finally 
            {
                _basePage.Dispose();
            }
        }
    }
    
}
