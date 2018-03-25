using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public class FirefoxDriverServicePool : IDisposable
    {
        private readonly ConcurrentBag<FirefoxDriverService> _pool = new ConcurrentBag<FirefoxDriverService>();

        public async Task<FirefoxDriverService> GetStartedServiceAsync()
        {
            if (!_pool.TryTake(out var service))
            {
                service = FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                await Task.Factory.StartNew(() => service.Start());
            }

            return service;
        }

        public void Release(FirefoxDriverService service)
        {
            _pool.Add(service);
        }

        public void Dispose()
        {
            while (_pool.TryTake(out var service))
            {
                service.Dispose();
            }
        }
    }
}
