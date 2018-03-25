using LaesoeLineApi;
using LaesoeLineApi.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenQA.Selenium
{
    public static class WebDriverExtensions
    {
        private static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        public static Task<TPage> GoToAsync<TPage>(this IWebDriver driver)
            where TPage : class, IPage
        {
            return Task.Factory.StartNew(() =>
            {
                var page = (TPage)Activator.CreateInstance(typeof(TPage), driver);

                driver.Url = page.Url;

                return page;
            });
        }

        public static async Task<IWebElement> TryFindVisibleElementAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            try
            {
                await WaitForElementToAppearAsync(driver, by, 1, timeout);

                return driver.FindElements(by).SingleOrDefault(x => x.Displayed);
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
        }

        public static async Task<IWebElement> FindVisibleElementAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            await WaitForElementToAppearAsync(driver, by, 1, timeout);

            return driver.FindElements(by).SingleOrDefault(x => x.Displayed);
        }

        public static async Task<SelectElement> FindVisibleSelectElementAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            var element = await FindVisibleElementAsync(driver, by, timeout);

            return new SelectElement(element);
        }

        public static Task WaitForElementToAppearAsync(this IWebDriver driver, By by, int minCount = 1, TimeSpan? timeout = null)
        {
            return Task.Factory.StartNew(() =>
            {
                var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);

                // ElementIsVisible @ https://github.com/SeleniumHQ/selenium/blob/29b5be0a72331df9ab99e36b05b4c29cf7046f30/dotnet/src/support/UI/ExpectedConditions.cs#L119
                wait.Until(d => d.FindElements(by).Where(x =>
                {
                    try
                    {
                        return x.Displayed;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                }).Count() >= minCount);
            });
        }

        public static Task WaitForElementToDisappearAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            return Task.Factory.StartNew(() =>
            {
                var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);

                // InvisibilityOfElementLocated @ https://github.com/SeleniumHQ/selenium/blob/29b5be0a72331df9ab99e36b05b4c29cf7046f30/dotnet/src/support/UI/ExpectedConditions.cs#L365
                wait.Until(d => d.FindElements(by).Where(x =>
                {
                    try
                    {
                        return x.Displayed;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return false;
                    }
                }).Count() == 0);
            });
        }

        public static Task WaitForTitleContainsAsync(this IWebDriver driver, string title, TimeSpan? timeout = null)
        {
            return Task.Factory.StartNew(() =>
            {
                var wait = new WebDriverWait(driver, timeout ?? DefaultTimeout);

                wait.Until(d => d.Title.Contains(title));
            });
        }

        public static void SetValueWithScript(this IWebDriver driver, string cssSelector, string value)
        {
            if (driver is WebDriverWrapper wrapper)
            {
                driver = wrapper.Inner;
            }

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
