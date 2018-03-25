using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public class FirefoxWebDriverFactory : IWebDriverFactory
    {
        private readonly IOptions<FirefoxSeleniumOptions> _options;

        public FirefoxWebDriverFactory(IOptions<FirefoxSeleniumOptions> options)
        {
            _options = options;
        }

        public async Task<IWebDriver> CreateAsync()
        {
            var firefoxOptions = new FirefoxOptions();

            if (_options.Value.Headless)
            {
                firefoxOptions.AddArgument("-headless");
            }

            return await Task.Factory.StartNew(() =>
            {
                var service = FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                service.Start();

                // https://github.com/SeleniumHQ/selenium/issues/4988
                var serviceUrl = new Uri(service.ServiceUrl.ToString().Replace("localhost", "127.0.0.1"));

                return new WebDriverWrapper(new RemoteWebDriver(serviceUrl, firefoxOptions), () => service.Dispose());
            });
        }
    }
}
