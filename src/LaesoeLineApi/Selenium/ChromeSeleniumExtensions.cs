using LaesoeLineApi.Selenium;
using System;

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
                .AddSingleton<IBrowserSessionFactory, ChromeBrowserSessionFactory>();

            return services;
        }
    }
}
