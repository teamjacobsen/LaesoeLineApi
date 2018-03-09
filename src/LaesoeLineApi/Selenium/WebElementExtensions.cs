using OpenQA.Selenium.Support.UI;

namespace OpenQA.Selenium
{
    public static class WebElementExtensions
    {
        public static void SelectByIndex(this IWebElement element, int index)
        {
            new SelectElement(element).SelectByIndex(index);
        }

        public static void SelectByValue(this IWebElement element, string value)
        {
            new SelectElement(element).SelectByValue(value);
        }

        public static void SetChecked(this IWebElement element, bool value)
        {
            if (element.Selected != value)
            {
                element.Click();
            }
        }
    }
}
