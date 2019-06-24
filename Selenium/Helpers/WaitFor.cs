
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.AutomationToolInterfaces;
using System;

namespace Selenium.Helpers
{
    public class WaitFor : IWaitFor
    {
        private IWebDriver _driver { get; set; }
        private By _locator { get; set; }
        private CustomJavaScriptExecuter JSExecuter;
        private const int TIMEOUT = 30;
        private const int POLLING_INTERVAL = 250;

        public WaitFor(IWebDriver driver, By locator)
        {
            _driver = driver;
            _locator = locator;
            JSExecuter = new CustomJavaScriptExecuter(_driver);
        }

        public void ElementPresent()
        {
            WaitForElementPresent(15);
        }

        public static void ThreeSeconds()
        {
            System.Threading.Thread.Sleep(3000);
        }

        public static void OneSecond()
        {
            System.Threading.Thread.Sleep(1000);
        }

        private bool IsStale(IWebDriver driver, By locator)
        {
            try
            {
                if (GetElement(driver, locator) != null)
                {
                    var tagName = GetElement(driver, locator).TagName;
                    return false;
                }
            }
            catch (StaleElementReferenceException)
            {
                System.Console.WriteLine("Element Is Stale => " + locator.ToString());
            }
            catch (Exception)
            {
                System.Console.WriteLine("Exception is Caught in IsStale() => " + locator.ToString());
            }
            return true;
        }

        private IWebElement GetElement(IWebDriver driver, By locator)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT/4);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => isElementAvailableNow(drv, _locator));
                return driver.FindElement(locator);
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in GetElement() => " + locator.ToString());
            }

            return null;
        }

        private IWebElement GetStableElement(IWebDriver driver, By locator)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT/2);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => !IsStale(drv, locator));
                return driver.FindElement(locator);
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in GetStableElement() => " + locator.ToString());
            }
            return null;
        }

        private string ExecuteThisScript(string script, object[] parameters)
        {
            return JSExecuter.Execute(script, parameters);
        }

        private string ExecuteThisScript(string script)
        {
            return JSExecuter.Execute(script, new object[] { });
        }

        public void MatchingObjectCountToBe(int count)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            wait.Until(drv => drv.FindElements(_locator).Count == count);
        }

        private string GetAttribute(IWebDriver driver, By locator, string attName)
        {
            string attValue = "";
            string script = "var elt = arguments[0];";
            script += "var attName = arguments[1];";
            script += "var attValue = elt.getAttribute(attName);";
            script += "return attValue;";


            attValue = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator), attName });
            //Retry to execute script again
            if (attValue == "" || attValue == "customException")
            {
                attValue = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator), attName });
                System.Console.WriteLine("Retried script Result [attValue] =>" + attValue);
            }

            if (attValue == null || attValue == "")
            {
                attValue = GetStableElement(driver, locator).GetAttribute(attName);
            }

            return attValue;
        }

        private bool doesElementClassNotContain(IWebDriver driver, By locator, string text)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    if (GetAttribute(driver, locator, "class").Contains(text))
                        return false;
                    else
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void ElementClassDoesNotContain(string text)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => doesElementClassNotContain(drv, _locator, text));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementClassDoesNotContain() => " + _locator.ToString() + "-" + text);
            }

        }

        private bool jQueryRunning()
        {
            string script = "if(window.jQuery) return 'true'; else return 'false';";
            string jQueryRun = ExecuteThisScript(script);
            if (jQueryRun == "true")
                return true;

            return false;
        }

        private bool isElementDisplayedNow(IWebDriver driver, By locator)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                 //   if (jQueryRunning())
                 //   {
                 //       string script = "var locator = arguments[0];";
                 //       script += "return $(locator).is(':visible').toString();";

                   //     string displayProp = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                        //Retry again
                   //     if (displayProp == "" || displayProp == "customException")
                    //    {
                     //       displayProp = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                      //      System.Console.WriteLine("Retried script Result [displayProp] =>" + displayProp);
                     //   }

                      //  if (displayProp == "true")
                     //       return true;
                   // }
                  //  else
                    //{
                        if (GetStableElement(driver, locator).Displayed)
                            return true;
                    //}
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }


        public void ElementDisplayed()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => isElementDisplayedNow(drv, _locator));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementDisplayed() => " + _locator.ToString());
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
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => isElementCount(drv, _locator, expCount));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ItsMatchingEltsCountToBecome() => " + _locator.ToString() + ", Count-" + expCount);
            }
        }

        public void MatchingObjectCountToBeMoreThanZero()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));

            wait.Until(drv => drv.FindElements(_locator).Count > 0);
        }

        public void ElementToBeClickable()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
            wait.Until(drv => isElementClickable(drv, _locator));
        }

        private bool isElementClickable(IWebDriver driver, By locator)
        {
            if (isElementAvailableNow(driver, locator))
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
                if (GetStableElement(drv, locator) != null)
                {
                    var color = GetCssValue(drv, locator, "background-color");
                    if (!color.StartsWith("#"))
                        color = GetHexValueOfColor(color);

                    if (color == expectedColorHexValue)
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void BackgroundColorToChangeTo(string expectedColorHexValue)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => isBackgroundColorChangedTo(drv, _locator, expectedColorHexValue));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in BackgroundColorToChangeTo() => " + _locator.ToString()
                                        + ", expectedColorHexValue-" + expectedColorHexValue);
            }
        }


        private bool isBorderColorChangedTo(IWebDriver driver, By locator, string expectedColorHexValue)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    var color = GetCssValue(driver, locator, "border-color");
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
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => isBorderColorChangedTo(drv, _locator, expectedColorHexValue));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in BackgroundColorToChangeTo() => " + _locator.ToString()
                                        + ", expectedColorHexValue-" + expectedColorHexValue);
            }
        }


        private bool isElementEnabled(IWebDriver driver, By locator)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    if (GetStableElement(driver, locator).Enabled)
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void SlideOptionToBecomeActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(drv => GetStableElement(drv, _locator).GetAttribute("tabindex").Contains("0"));
        }

        private bool isChildElementDisplayed(IWebDriver driver, By parentLocator, By childLocator)
        {
            try
            {
                if (GetStableElement(driver, parentLocator).FindElements(childLocator).Count > 0)
                    return true;
                else
                    return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


        public void ChildElementToDisplay(ITestObjectLocator childObjectLocator)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until(drv => isChildElementDisplayed(drv, _locator, childObjectLocator.GetLocator()));
        }

        public void ElementClassToContainValidated()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, "validated"));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementClassToContainValidated() => " + _locator.ToString());
            }
        }


        public void ElementClassToContainActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, "active"));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementClassToContainActive() => " + _locator.ToString());
            }
        }


        private bool doesElementClassContains(IWebDriver driver, By locator, string text)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    if (GetAttribute(driver, locator, "class").Contains(text))
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }


        public void ElementClassToContain(string text)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, text));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementClassToContain() => " + _locator.ToString() + ", text-" + text);
            }
        }

        private bool hasTheSlideCompletedTransition(IWebDriver driver, By locator)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    string script = "var element = arguments[0];";
                    script += "return element.style.transition;";

                    string transitionAtt = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                    //Retry again
                    if (transitionAtt == "" || transitionAtt == "customException")
                    {
                        transitionAtt = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                        System.Console.WriteLine("Retried script Result [transitionAtt] =>" + transitionAtt);
                    }


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
                if (GetStableElement(driver, locator) != null)
                {
                    string script = "var element = arguments[0];";
                    script += "return element.style.transition;";
                    string transitionAtt = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                    //Retry again
                    if (transitionAtt == "" || transitionAtt == "customException")
                    {
                        transitionAtt = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                        System.Console.WriteLine("Retried script Result [transitionAtt] =>" + transitionAtt);
                    }

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
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => hasTheSlideStartedTransition(drv, _locator));
                wait.Until(drv => hasTheSlideCompletedTransition(drv, _locator));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in SlideToCompleteTheTransition() => " + _locator.ToString());
            }
        }

        private string GetCssValue(IWebDriver driver, By locator, string propName)
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


            propValue = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator), propName });
            //Retry again
            if (propValue == "" || propValue == "customException")
            {
                propValue = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator), propName });
                System.Console.WriteLine("Retried script Result [propValue] =>" + propValue);
            }


            if (propValue == "" || propValue == null)
            {
                var style = GetStableElement(driver, locator).GetAttribute("style");
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
                    propValue = GetStableElement(driver, locator).GetCssValue(propName);
                }
            }

            return propValue;
        }

        private bool doesElementStyleContainsDisplayAs(IWebDriver driver, By locator, string text)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    if (GetCssValue(driver, locator, "display").Contains(text))
                        return true;
                }
            }
            catch (NoSuchElementException) { }
            return false;
        }

        public void StyleAttributeToContainDisplayBlock()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            wait.Until(drv => doesElementStyleContainsDisplayAs(drv, _locator, "block"));
        }

        public void StyleAttributeToNotContainDisplayNone()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

            wait.Until(drv => !doesElementStyleContainsDisplayAs(drv, _locator, "none"));
        }

        public void StyleAttributeToContainDisplayValueAs(string value)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            try
            {
                wait.Until(drv => doesElementStyleContainsDisplayAs(drv, _locator, value));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in StyleAttributeToContainDisplayValueAs() => " + _locator.ToString()
                                        + ", value - " + value);
            }
        }

        public void ElementClassToContainFlipped()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => doesElementClassContains(drv, _locator, "flipped"));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementClassToContainFlipped() => " + _locator.ToString());
            }
        }

        private bool isElementNotAvailableNow(IWebDriver driver, By locator)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
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
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => isElementNotAvailableNow(drv, _locator));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementToDisappear() => " + _locator.ToString());
            }
        }

        public void ElementClassDoesNotContainDisabled()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementClassNotContain(drv, _locator, "disabled"));

        }

        public void ElementClassDoesNotContainNotActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => doesElementClassNotContain(drv, _locator, "not-active"));

        }

        public void ElementClassDoesNotContainActive()
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);

            try
            {
                wait.Until(drv => doesElementClassNotContain(drv, _locator, "active"));
            }
            catch (NoSuchElementException) { }
            catch (WebDriverTimeoutException)
            {
                System.Console.WriteLine("TimeOut while trying in ElementClassDoesNotContainActive() => " + _locator.ToString());
            }
        }

        private bool isElementTextLoaded(IWebDriver driver, By locator)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    string script = "var element = arguments[0];";
                    script += "return element.innerText;";

                    string innerText = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                    //Retry again
                    if (innerText == "" || innerText == "customException")
                    {
                        innerText = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                        System.Console.WriteLine("Retried script Result [innerText] =>" + innerText);
                    }

                    if (innerText == "")
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
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isElementTextLoaded(drv, _locator));
        }


        private bool isElementTextLoadedWith(IWebDriver driver, By locator, string text)
        {
            try
            {
                if (GetStableElement(driver, locator) != null)
                {
                    string script = "var element = arguments[0];";
                    script += "return element.innerText;";
                    string innerText = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                    //Retry again
                    if (innerText == "" || innerText == "customException")
                    {
                        innerText = ExecuteThisScript(script, new object[] { GetStableElement(driver, locator) });
                        System.Console.WriteLine("Retried script Result [innerText] =>" + innerText);
                    }

                    if (innerText.Contains(text))
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
            wait.Timeout = System.TimeSpan.FromSeconds(TIMEOUT);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException),
                                      typeof(WebDriverTimeoutException));
            wait.Until(drv => isElementTextLoadedWith(drv, _locator, sText));
        }

        private void WaitForElementPresent(int seconds)
        {
            DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.Timeout = System.TimeSpan.FromSeconds(seconds);
            wait.PollingInterval = System.TimeSpan.FromMilliseconds(POLLING_INTERVAL);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(drv => isElementAvailableNow(drv, _locator));
        }

        private bool isElementAvailableNow(IWebDriver driver, By locator)
        {
            try
            {
                if (driver.FindElements(locator).Count > 0)
                    return true;
            }
            catch (Exception)
            {
                System.Console.WriteLine("Exception caught under isElementAvailableNow for Locator => " + locator.ToString());
            }
            return false;
        }

    }
}
