using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace LaesoeLineApi.Selenium
{
    public class WebDriverWrapper : IWebDriver
    {
        private readonly Action _dispose;

        public IWebDriver Inner { get; }

        public string Url { get => Inner.Url; set => Inner.Url = value; }

        public string Title => Inner.Title;

        public string PageSource => Inner.PageSource;

        public string CurrentWindowHandle => Inner.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => Inner.WindowHandles;

        public WebDriverWrapper(IWebDriver inner, Action dispose)
        {
            Inner = inner;
            _dispose = dispose;
        }

        public void Close()
        {
            Inner.Close();
        }

        public void Dispose()
        {
            Inner.Dispose();

            _dispose();
        }

        public IWebElement FindElement(By by)
        {
            return Inner.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return Inner.FindElements(by);
        }

        public IOptions Manage()
        {
            return Inner.Manage();
        }

        public INavigation Navigate()
        {
            return Inner.Navigate();
        }

        public void Quit()
        {
            Inner.Quit();
        }

        public ITargetLocator SwitchTo()
        {
            return Inner.SwitchTo();
        }
    }
}
