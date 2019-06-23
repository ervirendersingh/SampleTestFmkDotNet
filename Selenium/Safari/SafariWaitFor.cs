
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.AutomationToolInterfaces;

namespace Selenium.Safari
{
    public class SafariWaitFor : IWaitFor
    {
        private IWebDriver _driver { get; set; }
        private By _locator { get; set; }

        public SafariWaitFor(IWebDriver driver, By locator)
        {
            _driver = driver;
            _locator = locator;
        }
        public static void ThreeSeconds()
        {
            System.Threading.Thread.Sleep(3000);
        }

        public static void OneSecond()
        {
            System.Threading.Thread.Sleep(1000);
        }

        public void ElementPresent()
        {
            WaitForElementPresent(15);
        }

        public void MatchingObjectCountToBe(int count)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));

            wait.Until(drv => drv.FindElements(_locator).Count == count);
        }

        private string GetAttribute(IWebElement element, string attName)
        {
            string attValue = "";
            string script = "var locator = arguments[0];";
                script += "var attName = arguments[1];";
                script += "var attValue = locator.getAttribute(attName);";
                script += "return attValue;";

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            attValue = js.ExecuteScript(script, element, attName).ToString();
            
            if (attValue == null || attValue == "")
            {
                attValue = element.GetAttribute(attName);
            }

            return attValue;
        }

        private bool doesElementClassNotContain(IWebDriver driver, string text)
        {
            try
            {
                IWebElement element = driver.FindElement(_locator);
                if (element != null)
                {
                    if (GetAttribute(element, "class").Contains(text))
                        return false;
                    else
                        return true;
                }
            }
            catch(NoSuchElementException)
            { }
            return false;
        }

        public void ElementClassDoesNotContain(string text)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(5);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);

            try
            {
                wait.Until(drv => doesElementClassNotContain(drv, text));
            }
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }

        private bool jQueryRunning()
        {
            string script = "if(window.jQuery) return 'true'; else return 'false';";
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            string jQueryRun = js.ExecuteScript(script).ToString();
            if (jQueryRun == "true")
                return true;

            return false;
        }

        private bool isElementDisplayedNow(IWebDriver driver, By locator)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (jQueryRunning())
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                        string script = "var locator = arguments[0];";
                        script += "return $(locator).is(':visible');";
                        string displayProp = js.ExecuteScript(script, element).ToString();
                        if (displayProp == "True")
                            return true;
                    }
                    else
                    {
                        if (element.Displayed)
                            return true;
                    }
                }
            }
            catch(NoSuchElementException)
            {
            }
           return false; 
        }


        public void ElementDisplayed()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            try
            { 
                wait.Until(drv => isElementDisplayedNow(drv, _locator));
            }
            catch (NoSuchElementException)
            {
            }
            catch(WebDriverTimeoutException)
            {
            }         
        }

        private bool isElementCount(IWebDriver driver, By locator, int expCount)
        {
            int count = driver.FindElements(locator).Count;
            if (count == expCount)
            {
                return true;
            }

            return false;
        }

        public void ItsMatchingEltsCountToBecome(int expCount)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            try
            {
                wait.Until(drv => isElementCount(drv, _locator, expCount));
            }
            catch (NoSuchElementException)
            {
            }
            catch (WebDriverTimeoutException)
            {
            }
        }

        public void MatchingObjectCountToBeMoreThanZero()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));

            wait.Until(drv => drv.FindElements(_locator).Count>0);
        }

        public void ElementToBeClickable()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            wait.Until(drv => isElementClickable(drv, _locator));
        }

        private bool isElementClickable(IWebDriver driver, By locator)
        {
            if(isElementAvailableNow(driver, locator))
            {
                if (isElementEnabled(driver, locator))
                    return true;
            }
            return false;
        }

        private string GetHexValueOfColor(string rgbString)
        {
            string[] arr;
            if (rgbString.Contains("rgba"))
                arr = rgbString.Replace("rgba(", "").Replace(")", "").Split(',');
            else
                arr = rgbString.Replace("rgb(", "").Replace(")", "").Split(',');


            if (arr.Length < 3) return "";
            var r = int.Parse(arr[0].Trim()).ToString("X2");
            var g = int.Parse(arr[1].Trim()).ToString("X2");
            var b = int.Parse(arr[2].Trim()).ToString("X2");
            return "#" + r + g + b;
        }

        private bool isBackgroundColorChangedTo(IWebDriver drv, By locator, string expectedColorHexValue)
        {
            try
            {
                IWebElement element = drv.FindElement(locator);
                if (element != null)
                {
                    var color = GetCssValue(element, "background-color");
                    if (!color.StartsWith("#"))
                        color = GetHexValueOfColor(color);

                    if (color == expectedColorHexValue)
                        return true;
                }
            }
            catch(NoSuchElementException)
            { }
            return false;
        }

        public void BackgroundColorToChangeTo(string expectedColorHexValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(3);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);

            try
            {
                wait.Until(drv => isBackgroundColorChangedTo(drv, _locator, expectedColorHexValue));
            }
            catch (NoSuchElementException)
            {
            }
            catch (WebDriverTimeoutException)
            {
            }  
        }


        private bool isBorderColorChangedTo(IWebDriver drv, By locator, string expectedColorHexValue)
        {
            try
            {
                IWebElement element = drv.FindElement(locator);
                if (element != null)
                {
                    var color = GetCssValue(element, "border-color");
                    if (!color.StartsWith("#"))
                        color = GetHexValueOfColor(color);

                    if (color == expectedColorHexValue)
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }


        public void BorderColorToChangeTo(string expectedColorHexValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(3);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);

            try
            {
                wait.Until(drv => isBorderColorChangedTo(drv, _locator, expectedColorHexValue));
            }
            catch (NoSuchElementException)
            {
            }
            catch (WebDriverTimeoutException)
            {
            }
        }


        private bool isElementEnabled(IWebDriver driver, By locator)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (element.Enabled)
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void SlideOptionToBecomeActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => drv.FindElement(_locator).GetAttribute("tabindex").Contains("0"));
        }

        private bool isChildElementDisplayed(IWebDriver driver, By parentLocator, By childLocator)
        {
            try
            {
                if (driver.FindElement(parentLocator).FindElements(childLocator).Count > 0)
                    return true;
                else
                    return false;
            }
            catch(NoSuchElementException)
            {
                return false;
            }
        }


        public void ChildElementToDisplay(ITestObjectLocator childObjectLocator)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isChildElementDisplayed(drv, _locator, childObjectLocator.GetLocator()));    
        }

        public void ElementClassToContainValidated()
        {        
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(5);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(500);

            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, "validated"));
            }
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }


        public void ElementClassToContainActive()
        {            
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, "active"));
            }
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }


        private bool doesElementClassContains(IWebDriver driver, By locator, string text)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (GetAttribute(element, "class").Contains(text))
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }


        public void ElementClassToContain(string text)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, text));
            }
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }

        private bool hasTheSlideCompletedTransition(IWebDriver driver, By locator)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    string script = "var element = arguments[0];";
                    script += "return element.style.transition;";
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    string transitionAtt = js.ExecuteScript(script, element).ToString();
                    if (transitionAtt != "")
                        return false;
                    else
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        private bool hasTheSlideStartedTransition(IWebDriver driver, By locator)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    string script = "var element = arguments[0];";
                    script += "return element.style.transition;";
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    string transitionAtt = js.ExecuteScript(script, element).ToString();
                    if (transitionAtt != "")
                        return true;
                    else
                        return false;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void SlideToCompleteTheTransition()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(5);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            try
            {
                wait.Until(drv => hasTheSlideStartedTransition(drv, _locator));
                wait.Until(drv => hasTheSlideCompletedTransition(drv, _locator));
            }
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }

        private string GetCssValue(IWebElement element, string propName)
        {
            string propValue = "";
            string script = "var element = arguments[0];";
            script += "var prop = arguments[1];";
            script += "return getStyle(element, prop);";
            script += "function getStyle(oElm, css3Prop) {";
            script += "var strValue = '';";
            script += "if (window.getComputedStyle) {";
            script += "strValue = getComputedStyle(oElm).getPropertyValue(css3Prop);";
            script += "}";
            //IE
            script += "else if (oElm.currentStyle) {";
            script += "try";
            script += "{";
            script += "strValue = oElm.currentStyle[css3Prop];";
            script += "}";
            script += "catch (e) { }";
            script += "}";
            script += "return strValue;";
            script += "}";

            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            propValue = js.ExecuteScript(script, element, propName).ToString();

            if (propValue == "" || propValue == null)
            {
                var style = element.GetAttribute("style");
                if (style != "")
                {
                    var styleProperties = style.Split(';');
                    foreach (var property in styleProperties)
                    {
                        if (property.StartsWith(propName))
                        {
                            propValue = property.Split(':')[1];
                        }
                    }
                }
                else
                {
                    propValue = element.GetCssValue(propName);
                }
            }

            return propValue;
        }

        private bool doesElementStyleContainsDisplayAs(IWebDriver driver, By locator, string text)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (GetCssValue(element, "display").Contains(text))
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void StyleAttributeToContainDisplayBlock()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementStyleContainsDisplayAs(drv, _locator, "block"));
        }

        public void StyleAttributeToNotContainDisplayNone()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => !doesElementStyleContainsDisplayAs(drv, _locator, "none"));       
        }

        public void StyleAttributeToContainDisplayValueAs(string value)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            try
            {
                wait.Until(drv => doesElementStyleContainsDisplayAs(drv, _locator, value));
            }
            catch(NoSuchElementException)
            { }
            catch(WebDriverTimeoutException)
            { }
        }

        public void ElementClassToContainFlipped()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(5);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);

            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, "flipped"));
            }
            
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }

        private bool isElementNotAvailableNow(IWebDriver driver, By locator)
        {
            try
            {
                if (driver.FindElement(locator) != null)
                {
                    return false;
                }
            }
            catch (NoSuchElementException) { }
            return true;
        }

        public void ElementToDisappear()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);

            try
            {
                wait.Until(drv => isElementNotAvailableNow(drv, _locator));
            }
            catch(NoSuchElementException)
            {

            }
            catch(WebDriverTimeoutException)
            {

            }
        }


        private bool doesElementClassNotContain(IWebDriver driver, By locator, string text)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (GetAttribute(element, "class").Contains(text))
                        return false;
                    else
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void ElementClassDoesNotContainDisabled()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementClassNotContain(drv, _locator, "disabled"));
         
        }

        public void ElementClassDoesNotContainNotActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementClassNotContain(drv, _locator, "not-active"));
          
        }

        public void ElementClassDoesNotContainActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(5);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);

            try
            {
                wait.Until(drv => doesElementClassNotContain(drv, _locator, "active"));
            }
            catch (NoSuchElementException)
            {

            }
            catch (WebDriverTimeoutException)
            {

            }
        }

        private bool isElementTextLoaded(IWebDriver driver, By locator)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (element.Text == "")
                        return false;
                    else
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void ElementTextToLoad()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isElementTextLoaded(drv, _locator));
        }


        private bool isElementTextLoadedWith(IWebDriver driver, By locator, string text)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                if (element != null)
                {
                    if (element.Text.Contains(text))
                        return true;
                    else
                        return false;
                }
            }
            catch (NoSuchElementException) { }

            return false;
        }

        public void ElementTextToLoadWith(string sText)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(15);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(WebDriverTimeoutException));

            wait.Until(drv => isElementTextLoadedWith(drv, _locator, sText));
        }

        private void WaitForElementPresent(int seconds)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(seconds);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(250);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isElementAvailableNow(drv, _locator));     
        }

        private bool isElementAvailableNow(IWebDriver driver, By locator)
        {         
            if (driver.FindElements(locator).Count > 0)
                return true;
            else
                return false;
        }

    }
}
