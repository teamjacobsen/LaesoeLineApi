using LaesoeLineApi;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenQA.Selenium
{
    public static class WebDriverExtensions
    {
        private static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

        public static TPage GoTo<TPage>(this IWebDriver driver)
            where TPage : class, IPage
        {
            var page = (TPage)Activator.CreateInstance(typeof(TPage), driver);

            driver.Url = page.Url;

            return page;
        }

        public static IWebElement FindVisibleElement(this IWebDriver driver, By by)
        {
            return driver.FindElements(by).SingleOrDefault(x => x.Displayed);
        }

        public static IEnumerable<IWebElement> FindVisibleElements(this IWebDriver driver, By by)
        {
            return driver.FindElements(by).Where(x => x.Displayed);
        }

        public static void WaitForElementToAppear(this IWebDriver driver, By by, int minCount = 1, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);

            // https://github.com/SeleniumHQ/selenium/blob/29b5be0a72331df9ab99e36b05b4c29cf7046f30/dotnet/src/support/UI/ExpectedConditions.cs#L119
            wait.Until(drv =>
            {
                return drv.FindElements(by).Where(x =>
                {
                    try
                    {
                        return x.Displayed;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                }).Count() >= minCount;
            });
        }

        public static void WaitForElementToDisappear(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
        }

        public static void WaitForElementToDisappear(this IWebDriver driver, IWebElement element, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);

            // https://github.com/SeleniumHQ/selenium/blob/29b5be0a72331df9ab99e36b05b4c29cf7046f30/dotnet/src/support/UI/ExpectedConditions.cs#L365
            wait.Until(drv => {
                try
                {
                    return element == null || !element.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }

        public static void WaitForTitleContains(this IWebDriver driver, string title, TimeSpan? timeout = null)
        {
            var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);
            wait.Until(ExpectedConditions.TitleContains(title));
        }

        public static void SetValueWithScript(this IWebDriver driver, string cssSelector, string value)
        {
            if (driver is RemoteWebDriver remoteWebDriver)
            {
                remoteWebDriver.ExecuteScript($"document.querySelector('{cssSelector}').value='{value}'");
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
