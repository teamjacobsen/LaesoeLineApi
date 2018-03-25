using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FirefoxSeleniumExtensions
    {
        public static IServiceCollection AddFirefoxSeleniumWebDriver(this IServiceCollection services)
        {
            return AddFirefoxSeleniumWebDriver(services, null);
        }

        public static IServiceCollection AddFirefoxSeleniumWebDriver(this IServiceCollection services, Action<FirefoxSeleniumOptions> setupAction)
        {
            services.AddOptions();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services
                .AddSingleton<FirefoxDriverServicePool>()
                .AddSingleton<IWebDriverFactory, FirefoxWebDriverFactory>();

            return services;
        }
    }
}
