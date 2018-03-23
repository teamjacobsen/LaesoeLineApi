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
        private readonly ChromeDriverService _driverService;
        private readonly IOptions<ChromeSeleniumOptions> _options;

        public ChromeWebDriverFactory(ChromeDriverService driverService, IOptions<ChromeSeleniumOptions> options)
        {
            _driverService = driverService;
            _options = options;
        }

        public async Task<IWebDriver> CreateAsync()
        {
            lock (_driverService)
            {
                if (_driverService.ProcessId == 0)
                {
                    _driverService.Start();
                }
            }

            var chromeOptions = new ChromeOptions();

            if (_options.Value.Headless)
            {
                chromeOptions.AddArgument("--headless");
            }

            // https://github.com/SeleniumHQ/selenium/issues/4988
            //return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions)

            return await Task.Factory.StartNew(() => new RemoteWebDriver(new Uri($"http://127.0.0.1:{_options.Value.Port}"), chromeOptions));
        }
    }
}
