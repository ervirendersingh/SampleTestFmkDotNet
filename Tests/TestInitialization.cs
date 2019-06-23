using System.Configuration;
using System;
using System.IO;
using AppFmk.Interfaces;
using AppFmk.IOC;
using NUnit.Framework;

namespace Tests
{
    public class TestInitialization 
    {
        public IBasePage BasePage;
        public IFunctionExecuter Executer;
        public IReporter Reporter;

        public TestInitialization(IBasePage basePage, string fixtureName, IFunctionExecuter executer, IReporter reporter)
        {
            this.BasePage = basePage;
            this.Executer = executer;
            this.Reporter = reporter;
            BasePage.TestName = TestContext.CurrentContext.Test.Name;
            BasePage.FixtureName = fixtureName;
            BasePage.RunningBrowser = ConfigurationManager.AppSettings.Get("RunAllTestsOn");
            BasePage.ScreenShotsPath = CreateScreenShotPath();
        }

        private string CreateScreenShotPath()
        {
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string browserForAllTests = ConfigurationManager.AppSettings.Get("RunAllTestsOn");
            string dirPath;

            if (browserForAllTests != "")
                dirPath = Directory.GetCurrentDirectory() + "\\" + Todaysdate + "\\" + browserForAllTests + "\\" + "Screenshots";
            else
                dirPath = Directory.GetCurrentDirectory() + "\\" + Todaysdate + "\\" + "Screenshots";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return dirPath;
        }

      
    }
}
