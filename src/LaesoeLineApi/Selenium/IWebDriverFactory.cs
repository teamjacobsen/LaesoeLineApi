using OpenQA.Selenium;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public interface IWebDriverFactory
    {
        Task<IWebDriver> CreateAsync();
    }
}
