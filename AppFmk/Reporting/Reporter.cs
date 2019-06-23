

using AppFmk.Pages;
using AppFmk.Interfaces;
using AppFmk.IOC;
using System;
using System.Configuration;
using System.IO;

namespace AppFmk.Reporting
{
    public class Reporter : IReporter
    {
        public string FilePath { get; set; }
        public string TestName { get; set; }
        IBasePage BasePage;
        IResultPrinter ResultPrinter;
        private string _screenShotPath;

        public Reporter(IBasePage basePage, IResultPrinter resultPrinter)
        {
            this.BasePage = basePage;
            this.ResultPrinter = resultPrinter;
        }

        private void InitializeResultPrinter()
        {
            ResultPrinter.FilePath = CreateResultDirectoryAndFileIfNotExists(BasePage.FixtureName);
            ResultPrinter.TestName = BasePage.TestName;
            ResultPrinter.PrepareTemplate();
        }

        private string CreateResultDirectoryAndFileIfNotExists(string fixtureName)
        {
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string browserForAllTests = ConfigurationManager.AppSettings.Get("RunAllTestsOn");
            string filePath;

            if (browserForAllTests != "")
            {
                filePath = Directory.GetCurrentDirectory() + "\\" + Todaysdate + "\\" + browserForAllTests + "\\" + fixtureName + ".html";

                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\" + Todaysdate))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + Todaysdate);

                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\" + Todaysdate + "\\" + browserForAllTests))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + Todaysdate + "\\" + browserForAllTests);
            }
            else
            {
                filePath = Directory.GetCurrentDirectory() + "\\" + Todaysdate + "\\" + fixtureName + ".html";
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\" + Todaysdate))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\" + Todaysdate);
            }


            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            return filePath;
        }

        public void ReportResult(IVerificationInfo verifyInfo)
        {
            InitializeResultPrinter();
            string screenShotName="";
            if (verifyInfo.Result == false)
                screenShotName = BasePage.SaveScreenshot();

            if (screenShotName != "")
                _screenShotPath = "./Screenshots/" + screenShotName;
            else
                _screenShotPath = "";

            InsertResult(verifyInfo, _screenShotPath);       
        }

        public void ReportResult(bool Result)
        {
            InitializeResultPrinter();

            string screenShotName = "";
            screenShotName = BasePage.SaveScreenshot();

            if (screenShotName != "")
                _screenShotPath = "./Screenshots/" + screenShotName;
            else
                _screenShotPath = "";

            InsertResult(Result, _screenShotPath);
        }

        private void InsertResult(IVerificationInfo verifyInfo, string ssPath)
        {
            ResultPrinter.UpdateResult(verifyInfo.Result, verifyInfo.Actual, verifyInfo.Expected, verifyInfo.Message, ssPath);
        }

        private void InsertResult(bool result, string ssPath)
        {
            ResultPrinter.UpdateResult(result, "", "", "", ssPath);
        }
    }
}
