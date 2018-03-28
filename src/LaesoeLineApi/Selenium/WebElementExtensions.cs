using OpenQA.Selenium;
using System;

namespace LaesoeLineApi.Selenium
{
    public static class WebElementExtensions
    {
        public static bool IsDisplayed(this IWebElement element)
        {
            try
            {
                return element != null && element.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}
