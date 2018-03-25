using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public class ChromeWebDriverFactory : IWebDriverFactory
    {
        private readonly ChromeDriverServicePool _driverServicePool;
        private readonly IOptions<ChromeSeleniumOptions> _options;

        public ChromeWebDriverFactory(ChromeDriverServicePool driverServicePool, IOptions<ChromeSeleniumOptions> options)
        {
            _driverServicePool = driverServicePool;
            _options = options;
        }

        public async Task<IWebDriver> CreateAsync()
        {
            var service = await _driverServicePool.GetStartedServiceAsync();

            var chromeOptions = new ChromeOptions();

            if (_options.Value.Headless)
            {
                chromeOptions.AddArgument("--headless");
            }

            // https://github.com/SeleniumHQ/selenium/issues/4988
            var serviceUrl = new Uri(service.ServiceUrl.ToString().Replace("localhost", "127.0.0.1"));

            return await Task.Factory.StartNew(() => new WebDriverWrapper(new RemoteWebDriver(serviceUrl, chromeOptions), () => _driverServicePool.Release(service)));
        }
    }
}
