
using OpenQA.Selenium;
using System;
using System.IO;
using Selenium.AutomationToolInterfaces;

namespace Selenium.Helpers
{
    public class ScreenshotTaker : IScreenshotTaker
    {
        private IBrowser _browser;

        public ScreenshotTaker(IBrowser browser)
        {
            _browser = browser;
        }

        public void SaveScreenShotAt(string filePath)
        {
            filePath = filePath + ".jpeg";
            _browser.Focus();
            Screenshot ss = ((ITakesScreenshot)_browser.GetInstance().GetDriver<IWebDriver>()).GetScreenshot();
            filePath = GetUniquePath(filePath);
            ss.SaveAsFile(filePath, ScreenshotImageFormat.Jpeg);
        }

        private string GetUniquePath(string filePath)
        {
            if (!File.Exists(filePath))
                return filePath;

            Random rnd = new Random();
            filePath = filePath.Replace(".jpeg", "_" + rnd.Next(0, 99999).ToString()) + ".jpeg";
            return GetUniquePath(filePath);
        }
    }
}
