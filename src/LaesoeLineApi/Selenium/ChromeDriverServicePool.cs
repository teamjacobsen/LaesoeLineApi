using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public class ChromeDriverServicePool : IDisposable
    {
        private readonly ConcurrentBag<ChromeDriverService> _pool = new ConcurrentBag<ChromeDriverService>();

        public async Task<ChromeDriverService> GetStartedServiceAsync()
        {
            if (!_pool.TryTake(out var service))
            {
                service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                await Task.Factory.StartNew(() => service.Start());
            }

            return service;
        }

        public void Release(ChromeDriverService service)
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
