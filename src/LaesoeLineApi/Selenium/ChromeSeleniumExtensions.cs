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
        public static IServiceCollection AddChromeSeleniumWebDriver(this IServiceCollection services)
        {
            return AddChromeSeleniumWebDriver(services, null);
        }

        public static IServiceCollection AddChromeSeleniumWebDriver(this IServiceCollection services, Action<ChromeSeleniumOptions> setupAction)
        {
            services.AddOptions();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services
                .AddSingleton<ChromeDriverServicePool>()
                .AddSingleton<IWebDriverFactory, ChromeWebDriverFactory>();

            return services;
        }
    }
}
