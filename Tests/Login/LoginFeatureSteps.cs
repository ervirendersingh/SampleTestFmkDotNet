using System;
using TechTalk.SpecFlow;
using AppFmk.Pages;
using System.Configuration;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;
using AppFmk.Interfaces;
using AppFmk.IOC;
using Tests.DependenciesResolver;

namespace Tests
{
    [Binding]
    public class LoginFeatureSteps : TestInitialization
    {
        public LoginFeatureSteps(IBasePage basePage, IFunctionExecuter executer, IReporter reporter) :
           base(basePage, "LoginFeatureSteps", executer, reporter)
        {
        }

        [Given(@"I navigate to login page")]
        public void GivenINavigateToLoginPage()
        {
            Executer.Execute(() => BasePage.ShouldTakeMeToHomePage());
        }

        [Then(@"Login Page is displayed")]
        public void ThenLoginPageIsDisplayed()
        {
            Executer.Execute(() => Reporter.ReportResult(BasePage.LoginPage.IsDisplayed()));
        }

    }
}
