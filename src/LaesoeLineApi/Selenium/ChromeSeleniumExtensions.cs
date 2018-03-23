using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ChromeSeleniumExtensions
    {
        public static IServiceCollection AddChromeSeleniumWebDriver(this IServiceCollection services, Action<ChromeSeleniumOptions> setupAction)
        {
            services.AddOptions();
            services.Configure(setupAction);
            services
                .AddSingleton<IWebDriverFactory, ChromeWebDriverFactory>()
                .AddSingleton(x =>
                {
                    var options = x.GetRequiredService<IOptions<ChromeSeleniumOptions>>();

                    var driverService = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    driverService.Port = options.Value.Port;

                    return driverService;
                });

            return services;
        }
    }
}
