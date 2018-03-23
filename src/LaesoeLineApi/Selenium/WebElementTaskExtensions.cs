using System.Threading.Tasks;

namespace OpenQA.Selenium
{
    public static class WebElementTaskExtensions
    {
        public static Task ThenClick(this Task<IWebElement> elementTask) =>
            elementTask.ContinueWith(x => x.Result.Click());

        public static Task ThenSendKeys(this Task<IWebElement> elementTask, string text) =>
            elementTask.ContinueWith(x => x.Result.SendKeys(text));
    }
}
