using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
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
            services.AddSingleton(x =>
            {
                var options = x.GetRequiredService<IOptions<ChromeSeleniumOptions>>();

                var driverService = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                driverService.Port = options.Value.Port;

                return driverService;
            });
            services.AddTransient<IWebDriver>(x =>
            {
                var options = x.GetRequiredService<IOptions<ChromeSeleniumOptions>>();

                var driverService = x.GetRequiredService<ChromeDriverService>();

                lock (driverService)
                {
                    if (driverService.ProcessId == 0)
                    {
                        driverService.Start();
                    }
                }

                var chromeOptions = new ChromeOptions();

                if (options.Value.Headless)
                {
                    chromeOptions.AddArgument("--headless");
                }

                // https://github.com/SeleniumHQ/selenium/issues/4988
                //return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions)
               
                return new RemoteWebDriver(new Uri($"http://127.0.0.1:{options.Value.Port}"), chromeOptions);
            });

            return services;
        }
    }
}
