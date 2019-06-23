
namespace Selenium.AutomationToolInterfaces
{
    public interface IWaitFor
    {
        void ElementPresent();
        void ElementClassDoesNotContain(string text);
        void ElementDisplayed();
        void ItsMatchingEltsCountToBecome(int expCount);
        void MatchingObjectCountToBeMoreThanZero();
        void ElementToBeClickable();
        void BackgroundColorToChangeTo(string expectedColorHexValue);
        void BorderColorToChangeTo(string expectedColorHexValue);
        void SlideOptionToBecomeActive();
        void ChildElementToDisplay(ITestObjectLocator childLocator);
        void ElementClassToContainValidated();
        void ElementClassToContainActive();
        void ElementClassToContain(string text);
        void SlideToCompleteTheTransition();
        void StyleAttributeToContainDisplayBlock();
        void StyleAttributeToNotContainDisplayNone();
        void StyleAttributeToContainDisplayValueAs(string value);
        void ElementClassToContainFlipped();
        void ElementToDisappear();
        void ElementClassDoesNotContainDisabled();
        void ElementClassDoesNotContainNotActive();
        void ElementClassDoesNotContainActive();
        void ElementTextToLoad();
        void ElementTextToLoadWith(string sText);
        void MatchingObjectCountToBe(int count);
    }
}
