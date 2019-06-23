using System.Collections.Generic;
using System.Drawing;

namespace Selenium.AutomationToolInterfaces
{
    public interface ITestObject
    {
        ITestObjectLocator TestObjectLocator { get; set; }
        string GetDisplayStyle();
        void ScrollToViewElement();
        bool WaitForItsClassToNotContain(string sText);
        bool WaitTillItsClassContains(string sText);
        bool WaitForIt();
        string GetAttribute(string attName);
        bool WaitForStyleAttributeToNotContainDisplayNone();
        bool WaitForStyleAttributeToContainDisplayValueAs(string displayValue);
        bool WaitForMatchingItemsToPopulate();
        bool WaitTillMatchingObjectCountIs(int count);
        void WaitTillItsTextLoaded();
        void WaitTillItsTextLoadedWith(string sText);
        bool WaitForItsMatchingEltsCountToBecome(int expCount);
        bool WaitForElementToDisappear();
        bool ClickIfItsClickable();
        bool IsDisplayed();
        bool IsEnabled();
        Point GetLocation();
        bool IsSelected();
        bool IsChecked();
        Size GetSize();
        string GetTagName();
        string GetText();
        string GetFieldValue();
        void Click();
        void ClickFoward();
        void Clear();
        string GetCssValue(string propName);
        void SendKeys(string value);
        void Submit();
        void Select(string sValue);
        string GetSelectedValue();
        List<ITestObject> GetAllMatchingTestObjects();
        int GetAllMatchingTestObjectsCount();
        List<string> GetAllOptionsText();
        void SelectTheOneMatchingToText(string sText);
        List<string> GetAListOfAllMatchingTestObjectsTextsValues();
        void SelectMatchingItemNumber(int listItemNumber);
        List<string> GetAListOfAllMatchingTestObjectsTitleValues();
        void HighlightElement();
        string GetXPathTo<T>(T element);
        ITestObject GetParentItem();
        ITestObject GetChildItem(int itemNumber, string tagName);
        ITestObject GetChildTestObject(ITestObject to);
        ITestObject GetInputChildObject();
        void WaitForBackgroundColorToChangeTo(string expectedColorHexValue);
        void WaitForBorderColorToChangeTo(string expectedColorHexValue);
        string GetBackgroundHexValue();
        string GetBackgroundColorHexValue();
        string GetBorderBottomColorHexValue();
        string GetClassAttributeValue();
    }
}
