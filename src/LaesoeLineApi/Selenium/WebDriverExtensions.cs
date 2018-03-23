using LaesoeLineApi;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenQA.Selenium
{
    public static class WebDriverExtensions
    {
        private static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

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
            if (await TryWaitForElementToAppearAsync(driver, by, 1, timeout))
            {
                return driver.FindElements(by).SingleOrDefault(x => x.Displayed);
            }

            return null;
        }

        public static Task<bool> TryWaitForElementToAppearAsync(this IWebDriver driver, By by, int minCount = 1, TimeSpan? timeout = null)
        {
            // ElementIsVisible @ https://github.com/SeleniumHQ/selenium/blob/29b5be0a72331df9ab99e36b05b4c29cf7046f30/dotnet/src/support/UI/ExpectedConditions.cs#L119
            return TryWaitForAsync(() => driver.FindElements(by).Where(x =>
            {
                try
                {
                    return x.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            }).Count() >= minCount, timeout);
        }

        public static Task<bool> TryWaitForElementToDisappearAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            // InvisibilityOfElementLocated @ https://github.com/SeleniumHQ/selenium/blob/29b5be0a72331df9ab99e36b05b4c29cf7046f30/dotnet/src/support/UI/ExpectedConditions.cs#L365
            return TryWaitForAsync(() => driver.FindElements(by).Where(x =>
            {
                try
                {
                    return x.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            }).Count() == 0, timeout);
        }

        private static async Task<bool> TryWaitForAsync(Func<bool> condition, TimeSpan? timeout)
        {
            var fulfilled = false;
            var timeoutTime = DateTime.UtcNow.Add(timeout ?? DefaultTimeout);

            while (DateTime.UtcNow < timeoutTime)
            {
                fulfilled = condition();

                if (fulfilled)
                {
                    break;
                }

                await Task.Delay(50);
            }

            return fulfilled;
        }

        public static async Task<IWebElement> FindVisibleElementAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            var element = await TryFindVisibleElementAsync(driver, by, timeout);

            return element ?? throw new WebDriverTimeoutException();
        }

        public static async Task<SelectElement> FindVisibleSelectElementAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            var element = await FindVisibleElementAsync(driver, by, timeout);

            return new SelectElement(element);
        }

        public static async Task WaitForElementToAppearAsync(this IWebDriver driver, By by, int minCount = 1, TimeSpan? timeout = null)
        {
            if (!await TryWaitForElementToAppearAsync(driver, by, minCount, timeout))
            {
                throw new WebDriverTimeoutException();
            }
        }

        public static async Task WaitForElementToDisappearAsync(this IWebDriver driver, By by, TimeSpan? timeout = null)
        {
            if (!await TryWaitForElementToDisappearAsync(driver, by, timeout))
            {
                throw new WebDriverTimeoutException();
            }
        }

        public static async Task WaitForTitleContainsAsync(this IWebDriver driver, string title, TimeSpan? timeout = null)
        {
            if (!await TryWaitForAsync(() => driver.Title.Contains(title), timeout))
            {
                throw new WebDriverTimeoutException();
            }
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
