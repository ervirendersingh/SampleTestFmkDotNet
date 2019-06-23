using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Drawing;
using OpenQA.Selenium.Support.UI;
using Selenium.AutomationToolInterfaces;
using Selenium.IOC;
using Selenium.Enums;

namespace Selenium.InternetExplorer
{
    public class IETestObject : ITestObject
    {
        IToolDependencyResolver Resolver;
        IBrowser browser;
        IWaitFor WaitFor;
        public ITestObjectLocator TestObjectLocator { get; set; }

        public IETestObject(IBrowser browser, ITestObjectLocator toLocator, IToolDependencyResolver resolver)
        {
            this.browser = browser;
            Resolver = resolver;
            TestObjectLocator = toLocator;
            WaitFor = Resolver.Resolve<IWaitFor>(browser.GetInstance().GetDriver<IWebDriver>(),toLocator.GetLocator());
        }

        public By GetLocator()
        {
            return TestObjectLocator.GetLocator();
        }

        private IWebDriver GetDriver()
        {
            return browser.GetInstance().GetDriver<IWebDriver>();
        }

        private IWebElement GetAutomationElement()
        {
            WaitFor.ElementDisplayed();
            try
            {
                IWebElement element = GetDriver().FindElement(GetLocator());
                if (element != null)
                    focus(element);
                return element;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public bool WaitForItsClassToNotContain(string sText)
        {
            WaitFor.ElementClassDoesNotContain(sText);
            if (GetAttribute("class").Contains(sText))
                return false;
            else
                return true;
        }

        public IVerification VerifyDisplayStyleWith(string displayValue)
        {
            IVerification info =Resolver.Resolve<IVerification>();
            string script = "var element = arguments[0];";
            script += "return element.style.display;";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            string displayStyle = (string)js.ExecuteScript(script, GetAutomationElement());

            info.Expected = displayValue;
            info.Actual = displayStyle;
            if (displayStyle.Contains(displayValue))
                info.Result = true;
            else
                info.Result = false;

            return info;
        }

        public string GetDisplayStyle()
        {
            string script = "var element = arguments[0];";
            script += "return element.getAttribute('style');";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            string displayStyle =(string)js.ExecuteScript(script, GetAutomationElement());

            return displayStyle;
        }

        private IList<IWebElement> GetAutomationElements()
        {
            WaitFor.ElementDisplayed();
            return GetDriver().FindElements(GetLocator());
        }

        public bool WaitTillMatchingObjectCountIs(int count)
        {
            WaitFor.MatchingObjectCountToBe(count);
            if (GetAutomationElements().Count == count)
                return true;
            else
                return false;
        }

        public void SendDelayedKeys(string text)
        {
            string script = "var locator = arguments[0];";
            script += "var textToEnter =arguments[1].split('');";
            script += "var strLength =textToEnter.length-1;";
            script += "function delayedInput(x, max_x) {";
            script += "$(locator).val($(locator).val() + textToEnter[x]);";
            script += "if (max_x > 0){";
            script += "setTimeout(delayedInput, 2000, x + 1, max_x - 1);}";
            script += "else{";
            script += "return x;}}";
            script += "delayedInput(0, strLength);";
            script += "return 'true';";
            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript(script, TestObjectLocator.Value, text);
        }


        public void ScrollToViewElement()
        {
            string script = "var element = arguments[0];";
            script += "element.scrollIntoView();";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript(script, GetAutomationElement());
        }

        private void jQueryFocusOn(string locatorName, string locatorValue)
        {
            if (locatorName == "Id" || locatorName == "CssSelector")
            {
                if (locatorName == "Id")
                {
                    locatorValue = "#" + locatorValue;
                }
                string script = "var locator = arguments[0];";
                script += "$(locator).focus();";
                script += "return 'true';";
                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                js.ExecuteScript(script, locatorValue);
            }
        }

        private bool jQueryRunning()
        {
            string script = "if(window.jQuery) return 'true'; else return 'false';";
            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            string jQueryRun = (string)js.ExecuteScript(script);
            if (jQueryRun == "true")
                return true;

            return false;
        }

        private void focus(IWebElement element)
        {

                string script = "var element = arguments[0];";
                script += "window.focus();";
                script += "return element.focus();";
                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                js.ExecuteScript(script, element);
            //}
        }

        private IWebElement GetChildAutomationElementMatchingWith(ITestObjectLocator toLocator)
        {
            try
            {
                WaitFor.ChildElementToDisplay(toLocator);
                if (GetAutomationElement().FindElement(GetLocator()) != null)
                    return GetAutomationElement().FindElement(toLocator.GetLocator());
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            catch (WebDriverTimeoutException) { }
            return null;
        }


        private IList<IWebElement> GetChildAutomationElementsMatchingWith(ITestObjectLocator toLocator)
        {
            return GetAutomationElement().FindElements(toLocator.GetLocator());
        }

        public bool WaitTillItsClassContains(string sText)
        {
            WaitFor.ElementClassToContain(sText);
            if (GetAutomationElement() != null)
            {
                if (GetAutomationElement().GetAttribute("class").Contains(sText))
                    return true;
            }
            
            return false;
        }

        public bool WaitForIt()
        {
            if (GetAutomationElement() != null)
            {
                return IsThisElementVisible();
                // if (GetAutomationElement().Displayed)
                //     return true;
            }

            return false;
        }

      
        public string GetAttribute(string attName)
        {
            string attValue = "";
            if (jQueryRunning() && TestObjectLocator.LocateBy == Locator.CssSelector)
            {
                string script = "var locator = arguments[0];";
                script += "var attName = arguments[1];";
                script += "var attValue = $(locator).attr(attName);";
                script += "return attValue;";

                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                attValue = (string)js.ExecuteScript(script, TestObjectLocator.Value, attName);
            }
            else
            {
                string script = "var locator = arguments[0];";
                script += "var attName = arguments[1];";
                script += "var attValue = locator.getAttribute(attName);";
                script += "return attValue;";

                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                attValue = (string)js.ExecuteScript(script, GetAutomationElement(), attName);
            }

            if(attValue==null)
            {
                attValue = GetAutomationElement().GetAttribute(attName);
            }

            return attValue;
        }

        public bool WaitTillItsClassContainsValidated()
        {
            WaitFor.ElementClassToContainValidated();
            if (GetAttribute("class").Contains("validated"))
                return true;
            else
                return false;
        }


        public bool WaitTillItsClassContainsHide()
        {
            WaitFor.ElementClassToContain("hide");
            if (GetAttribute("class").Contains("hide"))
                return true;
            else
                return false;
        }

        public bool WaitForStyleAttributeToNotContainDisplayNone()
        {
            WaitFor.StyleAttributeToNotContainDisplayNone();
            if (GetCssValue("display").Contains("none"))
                return false;
            else
                return true;
        }

        public bool WaitForMatchingItemsToPopulate()
        {
            WaitFor.MatchingObjectCountToBeMoreThanZero();
            if (GetAllMatchingTestObjectsCount()>0)
                return true;
            else
                return false;
        }

        public bool WaitForStyleAttributeToContainDisplayValueAs(string displayValue)
        {
            WaitFor.StyleAttributeToContainDisplayValueAs(displayValue);
            if (GetAutomationElement() != null)
            {
                if (GetCssValue("display").Contains(displayValue))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool WaitTillItsClassContainsFlipped()
        {
            WaitFor.ElementClassToContainFlipped();
            if (GetAttribute("class").Contains("flipped"))
                return true;
            else
                return false;
        }

        public void WaitTillItsTextLoaded()
        {
            WaitFor.ElementTextToLoad();
        }

        public void WaitTillItsTextLoadedWith(string sText)
        {
            WaitFor.ElementTextToLoadWith(sText);
        }

        public bool WaitForItsClassToNotContainDisabled()
        {
            WaitFor.ElementClassDoesNotContainDisabled();
            if (GetAttribute("class").Contains("disabled"))
                return false;
            else
                return true;
        }

        public bool WaitForItsClassToNotContainActive()
        {
            WaitFor.ElementClassDoesNotContainActive();
            if (GetAttribute("class").Contains("active"))
                return false;
            else
                return true;
        }


        public bool WaitForItsClassToNotContainNotActive()
        {
            WaitFor.ElementClassDoesNotContainNotActive();
            if (GetAttribute("class").Contains("not-active"))
                return false;
            else
                return true;
        }

        public bool WaitForItsMatchingEltsCountToBecome(int expCount)
        {
            WaitFor.ItsMatchingEltsCountToBecome(expCount);
            if (GetAllMatchingTestObjectsCount() == expCount)
                return true;
            else
                return false;
        }

        public bool WaitForElementToDisappear()
        {
            WaitFor.ElementToDisappear();
            return true;
        }


        public bool WaitForSlideOptionToBecomeActive()
        {
            WaitFor.SlideOptionToBecomeActive();
            if (GetAttribute("tabindex").Contains("0"))
                return true;
            else
                return false;
        }

        public bool ClickIfItsClickable()
        {
            if (GetAutomationElement() != null)
            {
                try
                {
                    GetAutomationElement().Click();
                }
                catch (ElementNotVisibleException e)
                {
                    return false;
                }
            }
            else
                return false;

            return true;
        }


        public bool IsDisplayed()
        {
            if (GetAutomationElement() != null)
            {
                if(jQueryRunning())
                    return IsThisElementVisible();
                else
                    return GetAutomationElement().Displayed;
            }
            else
                return false;
        }

        public bool IsEnabled()
        {
            if (GetAutomationElement() != null)
            {
                return GetAutomationElement().Enabled;
            }
            else
                return false;
        }

        public Point GetLocation()
        {
            return GetAutomationElement().Location;
        }

        public bool IsSelected()
        {
            return GetAutomationElement().Selected;
        }

        public bool IsChecked()
        {
            return (GetAttribute("checked") == "checked");
        }

        public Size GetSize()
        {
            return GetAutomationElement().Size;
        }

        public string GetTagName()
        {
            return GetAutomationElement().TagName;
        }

        public string GetText()
        {
            if (GetAutomationElement() != null)
            {
                //if (IsThisElementVisible())
                //{
                    string textValue = "";
                    if (jQueryRunning() && TestObjectLocator.LocateBy == Locator.CssSelector)
                    {
                        string script = "var locator = arguments[0];";
                        script += "var text = $(locator).text();";
                        script += "return text;";

                        IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                        textValue = (string)js.ExecuteScript(script, TestObjectLocator.Value);
                    }
                    else
                    {
                        string script = "var locator = arguments[0];";
                        script += "var text = locator.innerText;";
                        script += "return text;";

                        IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                        textValue = (string)js.ExecuteScript(script, GetAutomationElement());
                    }

                    if(textValue == null)
                        textValue = GetAutomationElement().Text.Trim();

                    return textValue.Trim();
                //}
            }

            return "error";
        }

        public void ShowAddressFields()
        {
            string script = "var locator = arguments[0];";
            script += "var className = $(locator).attr('class');";
            script += "var updatedClassName = className+' show-edit';";
            script += "$(locator).attr('class', updatedClassName);";
            script += "$('a.edit-address').click();";
            script += "return 'true';";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript(script, TestObjectLocator.Value);
        }

        public void ClearMobileNumberField()
        {
            string script = "var locator = arguments[0];";
            script += "$(locator).val('');";
            script += "return 'true';";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript(script, TestObjectLocator.Value);
        }

        public string GetFieldValue()
        {
            HighlightElement();
            string script = "var locator = arguments[0];";
            script += "var value = locator.value;";
            script += "return value;";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            string value =(string) js.ExecuteScript(script, GetAutomationElement());
            return value;
        }

        public void Click()
        {
            WaitFor.ElementToBeClickable();
          
            SimulateDOMClick();
        }

        public void JQueryClick()
        {
            if (TestObjectLocator.LocateBy.ToString() == "Id" || TestObjectLocator.LocateBy == Locator.CssSelector)
            {
                if (TestObjectLocator.LocateBy.ToString() == "Id")
                {
                    TestObjectLocator.Value = "#" + TestObjectLocator.Value;
                }
                string script = "var locator = arguments[0];";
                script += "$(locator).click();";
                script += "return 'true';";

                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                js.ExecuteScript(script, TestObjectLocator.Value);
            }
        }

        public void ClickFoward()
        {
            string script = "var locator = arguments[0];";
            script += "$(locator).click();";
            script += "return 'true';";

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript(script, TestObjectLocator.Value);
        }

        public void SimulateDOMClick()
        {

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript("arguments[0].click();", GetAutomationElement());
        }


        public void Clear()
        {
            GetAutomationElement().Clear();
        }

        public string GetCssValue(string propName)
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

                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                propValue = (string)js.ExecuteScript(script, GetAutomationElement(), propName);

                if (propValue == "" || propValue == null)
                {
                    var style = GetAutomationElement().GetAttribute("style");
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
                        propValue = GetAutomationElement().GetCssValue(propName);
                    }
                }
           // }

            return propValue;
        }

        public void SeleniumNativeSendKeys(string value)
        {
            Clear();
            GetAutomationElement().SendKeys(value);
        }

        public void SendKeysToMobileField(string value)
        {
            SeleniumNativeSendKeys(value);
           
        }


        public void SendKeys(string value)
        {

              string script = "var element = arguments[0];";
              script += "var textToSet = arguments[1];";
              script += "element.focus();";
              script += "element.value = textToSet;";
              script += "return 'true';";

              IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
              js.ExecuteScript(script, GetAutomationElement(), value);
             
        }

        public void Submit()
        {
            GetAutomationElement().Submit();
        }

        public void Select(string sValue)
        {

            SelectElement element = new SelectElement(GetAutomationElement());
            element.SelectByText(sValue);
        }

        public string GetSelectedValue()
        {
            SelectElement element = new SelectElement(GetAutomationElement());
            return element.SelectedOption.Text;
        }

        public List<ITestObject> GetAllMatchingTestObjects()
        {
            List<ITestObject> testObjectList = new List<ITestObject>();
            ITestObjectFactory factory = Resolver.Resolve<ITestObjectFactory>(browser);
            foreach (var element in GetAutomationElements())
            {
                string locatorValue = GetXPathTo(element);            
                testObjectList.Add(factory.Get(Locator.XPath, locatorValue));
            }

            return testObjectList;
        }

        public int GetAllMatchingTestObjectsCount()
        {
            return GetAutomationElements().Count;
        }

        public List<string> GetAllOptionsText()
        {
            List<string> options = new List<string>();

            SelectElement selElement = new SelectElement(GetAutomationElement());

            foreach (IWebElement optionElt in selElement.Options)
            {
                options.Add(optionElt.Text);
            }
            return options;
        }

        public void SelectTheOneMatchingToText(string sText)
        {
            foreach (var testObject in GetAllMatchingTestObjects())
            {
                if (testObject.GetText() == sText)
                {
                    testObject.GetInputChildObject().Click();
                }
            }

        }

        public ITestObject GetInputChildObject()
        {
            ITestObjectLocator inputObjectLocator = Resolver.Resolve<ITestObjectLocator>(Locator.TagName, "input");

            ITestObjectLocator inputChildTestObjectLocator = Resolver.Resolve<ITestObjectLocator>(Locator.XPath,
                                                                                                GetXPathTo(GetChildAutomationElementMatchingWith(inputObjectLocator)));

            ITestObjectFactory toFactory = Resolver.Resolve<ITestObjectFactory>(browser);
            return toFactory.Get(inputChildTestObjectLocator);
        }

        public List<string> GetAListOfAllMatchingTestObjectsTextsValues()
        {
            List<string> textList = new List<string>();
            foreach (var testObject in GetAllMatchingTestObjects())
            {
                textList.Add(testObject.GetText());
            }

            return textList;
        }

        public void SelectMatchingItemNumber(int listItemNumber)
        {
            GetAllMatchingTestObjects()[listItemNumber - 1].Click();
        }

        public List<string> GetAListOfAllMatchingTestObjectsTitleValues()
        {
            List<string> textList = new List<string>();
            foreach (var testObject in GetAllMatchingTestObjects())
            {
                textList.Add(testObject.GetAttribute("title"));
            }

            return textList;
        }


        public override string ToString()
        {
            return GetAutomationElement().ToString();
        }

        public void HighlightElement()
        {
            for (int i = 0; i < 2; i++)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", GetAutomationElement(), "border: 2px solid red;");
                js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", GetAutomationElement(), "");
            }

        }

        public string GetXPathTo<IWebElement>(IWebElement element)
        {

            string script = "function getPathTo(element){";
            script += "if (element.tagName == 'HTML')";
            script += "return '/HTML';";
            script += "if (element === document.body)";
            script += " return '/HTML/BODY';";
            script += "var index = 0;";
            script += "var siblings = element.parentNode.childNodes;";
            script += "for (var i = 0; i < siblings.length; i++){";
            script += "var sibling = siblings[i];";
            script += "if (sibling === element)";
            script += "return getPathTo(element.parentNode) + '/' + element.tagName + '[' + (index + 1) + ']';";
            script += "if (sibling.nodeType === 1 && sibling.tagName === element.tagName)";
            script += "index++;}}";
            script += "return getPathTo(arguments[0]);";



            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();

            string xpath =(string) js.ExecuteScript(script, element);
            if (xpath.EndsWith("[1]"))
                xpath = xpath.Substring(0, xpath.Length - 3);

            return xpath;
        }

        private string GetParentElementXPath(IWebElement element)
        {
            string script = "function getPathTo(element){";
            script += "if (element.tagName == 'HTML')";
            script += "return '/HTML';";
            script += "if (element === document.body)";
            script += " return '/HTML/BODY';";
            script += "var index = 0;";
            script += "var siblings = element.parentNode.childNodes;";
            script += "for (var i = 0; i < siblings.length; i++){";
            script += "var sibling = siblings[i];";
            script += "if (sibling === element)";
            script += "return getPathTo(element.parentNode) + '/' + element.tagName + '[' + (index + 1) + ']';";
            script += "if (sibling.nodeType === 1 && sibling.tagName === element.tagName)";
            script += "index++;}}";
            script += "return getPathTo(arguments[0].parentNode);";



            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();

            string xpath = (string)js.ExecuteScript(script, element);
            if (xpath.EndsWith("[1]"))
                xpath = xpath.Substring(0, xpath.Length - 3);

            return xpath;
        }


        public ITestObject GetParentItem()
        {
            ITestObjectLocator parentObjectLocator = Resolver.Resolve<ITestObjectLocator>(Locator.XPath,
                                                                                        GetParentElementXPath(GetAutomationElement()));

            ITestObjectFactory toFactory = Resolver.Resolve<ITestObjectFactory>(browser);
            return toFactory.Get(parentObjectLocator);
        }

        public ITestObject GetChildItem(int itemNumber, string tagName)
        {
            ITestObjectLocator objectLocator = Resolver.Resolve<ITestObjectLocator>(Locator.TagName,
                                                                                    tagName);

            ITestObjectLocator childObjectLocator = Resolver.Resolve<ITestObjectLocator>(Locator.XPath,
                                                                                        GetXPathTo(GetChildAutomationElementsMatchingWith(objectLocator)[itemNumber - 1]));

            ITestObjectFactory toFactory = Resolver.Resolve<ITestObjectFactory>(browser);
            return toFactory.Get(childObjectLocator);
        }


        public ITestObject GetChildTestObject(ITestObject to)
        {
            if (GetChildAutomationElementMatchingWith(to.TestObjectLocator) != null)
            {
                ITestObjectLocator childObjectLocator = Resolver.Resolve<ITestObjectLocator>(Locator.XPath,
                                                                                            GetXPathTo(GetChildAutomationElementMatchingWith(to.TestObjectLocator)));
                ITestObjectFactory toFactory = Resolver.Resolve<ITestObjectFactory>(browser);
                return toFactory.Get(childObjectLocator);
            }

            return null;
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

        public void WaitForBackgroundColorToChangeTo(string expectedColorHexValue)
        {
            WaitFor.BackgroundColorToChangeTo(expectedColorHexValue);
        }

        public void WaitForBorderColorToChangeTo(string expectedColorHexValue)
        {
            WaitFor.BorderColorToChangeTo(expectedColorHexValue);
        }

        public string GetBackgroundHexValue()
        {
            var color = GetCssValue("background");
            return color.StartsWith("#") ? color : GetHexValueOfColor(color);
        }

        public string GetBackgroundColorHexValue()
        {
            var color = GetCssValue("background-color");
            return color.StartsWith("#") ? color : GetHexValueOfColor(color);
        }

        public string GetBorderBottomColorHexValue()
        {
            var color = GetCssValue("border-bottom-color");
            return color.StartsWith("#") ? color : GetHexValueOfColor(color);
        }

  
   
        private string GetBorderColorHexValue()
        {
            string borderColor = "";
            borderColor = GetCssValue("border-color");
           
            borderColor = borderColor.StartsWith("#") ? borderColor : GetHexValueOfColor(borderColor);

            return borderColor;
        }

        private string GetTestObjectColorHexValue()
        {
            var color = GetCssValue("color");
            return color.StartsWith("#") ? color : GetHexValueOfColor(color);
        }

        public string GetClassAttributeValue()
        {
            return GetAttribute("class");
        }

       

        private bool IsThisElementVisible()
        {
            string script = "var locator = arguments[0];";
            script += "return $(locator).is(':visible').toString();";
            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            string visibleProp = (string)js.ExecuteScript(script, GetAutomationElement());
            if (visibleProp == "true")
                return true;
            else
                return false;
        }

       
    }
}