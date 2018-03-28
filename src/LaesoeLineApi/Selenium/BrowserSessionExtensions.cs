using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public static class BrowserSessionExtensions
    {
        private static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        public static Task WaitForInteractiveReadyStateAsync(this IBrowserSession session)
        {
            return session.InvokeAsync(driver =>
            {
                var wait = new WebDriverWait(driver, DefaultTimeout);

                wait.Until(_ => (bool?)driver.ExecuteScript("return (document.readyState === 'interactive' || document.readyState === 'complete') && window.jQuery && window.jQuery.active === 0") == true);
            });
        }

        public static async Task<bool> TryInvokeOnElementAsync(this IBrowserSession session, By by, Action<IWebElement> handler)
        {
            try
            {
                await WaitForElementToAppearAsync(session, by, 1);

                await session.InvokeAsync(driver =>
                {
                    var element = driver.FindElements(by).Single(x => x.IsDisplayed());

                    handler(element);
                });

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static async Task InvokeOnElementAsync(this IBrowserSession session, By by, Action<IWebElement> handler)
        {
            await WaitForElementToAppearAsync(session, by, 1);

            await session.InvokeAsync(driver =>
            {
                var element = driver.FindElements(by).Single(x => x.IsDisplayed());

                handler(element);
            });
        }

        public static Task InvokeOnSelectElementAsync(this IBrowserSession session, By by, Action<SelectElement> handler)
        {
            return InvokeOnElementAsync(session, by, x => handler(new SelectElement(x)));
        }

        public static Task SetValueWithScriptAsync(this IBrowserSession session, string cssSelector, string value)
        {
            return session.InvokeAsync(driver => driver.ExecuteScript($"document.querySelector('{cssSelector}').value='{value}'"));
        }

        public static Task WaitForElementToAppearAsync(this IBrowserSession session, By by, int minCount = 1)
        {
            return WaitForAsync(session, driver => driver.FindElements(by).Where(x => x.IsDisplayed()).Count() >= minCount);
        }

        public static Task WaitForElementToDisappearAsync(this IBrowserSession session, By by)
        {
            return WaitForAsync(session, driver => driver.FindElements(by).Where(x => x.IsDisplayed()).Count() == 0);
        }

        public static Task WaitForAsync(this IBrowserSession session, Func<RemoteWebDriver, bool> condition)
        {
            return session.InvokeAsync(driver =>
            {
                var clock = new SystemClock();
                var wait = new WebDriverWait(clock, driver, DefaultTimeout, TimeSpan.FromMilliseconds(50));

                wait.Until(_ => condition(driver));
            });
        }
    }
}
