using OpenQA.Selenium.Support.UI;
using System.Threading.Tasks;

namespace OpenQA.Selenium
{
    public static class SelectElementTaskExtensions
    {
        public static Task ThenSelectByIndex(this Task<SelectElement> elementTask, int index) =>
            elementTask.ContinueWith(x => x.Result.SelectByIndex(index));

        public static Task ThenSelectByValue(this Task<SelectElement> elementTask, string value) =>
            elementTask.ContinueWith(x => x.Result.SelectByValue(value));
    }
}
