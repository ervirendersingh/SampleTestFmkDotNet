
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Selenium.Helpers
{
    public class WaitFor
    {
        private IWebDriver _driver;
        private By _locator;


        private bool isElementPresent()
        {
            try
            {
                if (_driver.FindElement(_locator) != null)
                {
                    System.Console.WriteLine("Found - " + _locator);
                    return true;
                }
            }
            catch(NoSuchElementException)
            {
                System.Console.WriteLine("Not found - " + _locator);
                return false;
            }

            return false;
        }

        public static void ThreeSeconds()
        {
            System.Threading.Thread.Sleep(3000);
        }

        public static void OneSecond()
        {
            System.Threading.Thread.Sleep(1000);
        }


        public WaitFor(IWebDriver driver, By locator)
        {
            _driver = driver;
            _locator = locator;
        }

        public void ElementPresent()
        {
            WaitForElementPresent(15);
        }

        public void ElementDisplayed()
        {
        
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            try
            {
                wait.Until(drv => isElementAvailableNow(drv));
            }
            catch (NoSuchElementException) { System.Console.WriteLine("Not found - " + _locator); }
            catch (WebDriverTimeoutException) { System.Console.WriteLine("Timeout for  - " + _locator); }

            if (isElementAvailableNow(_driver))
            {
                try
                {
                    wait.Until(drv => drv.FindElement(_locator).Displayed);
                }
                catch (NoSuchElementException) { System.Console.WriteLine("Not found - " + _locator); }
                catch (WebDriverTimeoutException) { System.Console.WriteLine("Timeout for found - " + _locator); }         
            }
        }


        public void ElementToBeClickable()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(3));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            //wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
            wait.Until(drv => isElementClickable(drv));
        }

        private bool isElementClickable(IWebDriver driver)
        {
            if(isElementAvailableNow(driver))
            {
                if (isElementEnabled())
                    return true;
            }
            return false;
        }

        private bool isElementEnabled()
        {
            if (_driver.FindElement(_locator).Enabled)
                    return true;

            return false;
        }

        public void SlideOptionToBecomeActive()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => drv.FindElement(_locator).GetAttribute("tabindex").Contains("0"));
        }


        private bool isChildElementDisplayed(IWebDriver driver, By childLocator)
        {
            if (driver.FindElement(_locator).FindElements(childLocator).Count > 0)
                return true;
            else
                return false;
        }


        public void ChildElementToDisplay(By childLocator)
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isChildElementDisplayed(drv, childLocator));    
        }


        public void ElementClassToContainActive()
        {
            // IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            ElementPresent();
                //wait.Until(drv => drv.FindElement(_locator).GetAttribute("class").Contains("active"));                
                IWebElement element = _driver.FindElement(_locator);
                DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
                wait.Timeout = System.TimeSpan.FromSeconds(15);
                wait.PollingInterval = System.TimeSpan.FromSeconds(1);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
                {
                    string classAttrib = element.GetAttribute("class");
                    if (classAttrib.Contains("active"))
                    {
                        return true;
                    }
                    return false;
                });

                wait.Until(waiter);
        }

        public void StyleAttributeToContainDisplayBlock()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => drv.FindElement(_locator).GetAttribute("style") == "display: block;");
        }

        public void StyleAttributeToNotContainDisplayNone()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => drv.FindElement(_locator).GetAttribute("style") == "");
        
        }


        private bool isStyleAttributeContainDisplayValueAs(IWebDriver driver, string value)
        {
            string script = "var element = arguments[0];";
            script += "return element.getAttribute('style');";

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            IWebElement element = driver.FindElement(_locator);

            string displayStyle = js.ExecuteScript(script, element).ToString();
            if (displayStyle.Contains(value))
                return true;
            else
                return false;
        }

        public void StyleAttributeToContainDisplayValueAs(string value)
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isStyleAttributeContainDisplayValueAs(drv, value)); 
        }

        public void ElementClassToContainFlipped()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => drv.FindElement(_locator).GetAttribute("class").Contains("flipped"));
         
        }

        private bool isElementNotAvailableNow(IWebDriver drv)
        {
            if (isElementPresent())
            {
                System.Console.WriteLine(_locator.ToString()+" element available");
                return false;
            }
            else
                System.Console.WriteLine(_locator.ToString() + " element not available");

            return true;
        }

        public void ElementToDisappear()
        {
            System.Console.WriteLine("Enter Element To Disappear");
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException));

            wait.Until(drv => isElementNotAvailableNow(drv));

            System.Console.WriteLine("Exit Element To Disappear");
        }


        private bool doesElementClassNotContainDisabled(IWebDriver driver)
        {
            if (driver.FindElement(_locator).GetAttribute("class").Contains("disabled"))
                return false;
            else
                return true;
        }

        public void ElementClassDoesNotContainDisabled()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementClassNotContainDisabled(drv));
         
        }

        private bool doesElementClassNotContainNotActive(IWebDriver driver)
        {
            if (driver.FindElement(_locator).GetAttribute("class").Contains("not-active"))
                return false;
            else
                return true;
        }
        public void ElementClassDoesNotContainNotActive()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementClassNotContainNotActive(drv));
          
        }


        private bool isElementTextLoaded(IWebDriver driver)
        {
            if (driver.FindElement(_locator).Text == "")
                return false;
            else
                return true;
        }

        public void ElementTextToLoad()
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isElementTextLoaded(drv));
        }


        private bool isElementTextLoadedWith(IWebDriver driver, string text)
        {
            if (driver.FindElement(_locator).Text.Contains(text))
                return false;
            else
                return true;
        }

        public void ElementTextToLoadWith(string sText)
        {
            //IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(7));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException));

            wait.Until(drv => isElementTextLoadedWith(drv, sText));
        }
        private void WaitForElementPresent(int seconds)
        {
            // IWait<IWebDriver> wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(seconds));
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(seconds);
            wait.PollingInterval = System.TimeSpan.FromSeconds(1);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isElementAvailableNow(drv));     
        }

        private bool isElementAvailableNow(IWebDriver driver)
        {
            if (isElementPresent())
                return true;
            else
                return false;
        }

    }
}
