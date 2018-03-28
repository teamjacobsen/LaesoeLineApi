using LaesoeLineApi.Selenium;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace LaesoeLineApi.Features
{
    public abstract class PageBase : IPage
    {
        private readonly IBrowserSession _session;
        private readonly ILogger _logger;

        public abstract string Url { get; }

        public PageBase(IBrowserSession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
        }

        protected async Task ExecuteWithRetry(Func<Task> handler)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    await handler();

                    return;
                }
                catch (WebDriverTimeoutException e)
                {
                    _logger.LogWarning(e, "Timeout");

                    await _session.GoToAsync(Url);
                }
            }

            throw new ApiException(ApiStatus.GatewayTimeout);
        }

        protected async Task<T> ExecuteWithRetry<T>(Func<Task<T>> handler)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    return await handler();
                }
                catch (WebDriverTimeoutException e)
                {
                    _logger.LogWarning(e, "Timeout");

                    await _session.GoToAsync(Url);
                }
            }

            throw new ApiException(ApiStatus.GatewayTimeout);
        }
    }
}
