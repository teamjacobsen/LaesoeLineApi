using OpenQA.Selenium.Remote;
using System;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public interface IBrowserSession : IDisposable
    {
        Task GoToAsync(string url);
        Task<TPage> GoToAsync<TPage>() where TPage : IPage;
        Task InvokeAsync(Action<RemoteWebDriver> handler);
        Task<TResult> InvokeAsync<TResult>(Func<RemoteWebDriver, TResult> handler);
    }
}
