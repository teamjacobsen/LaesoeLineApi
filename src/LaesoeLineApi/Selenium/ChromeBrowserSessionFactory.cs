using Microsoft.Extensions.DependencyInjection;
using System;

namespace LaesoeLineApi.Selenium
{
    public class ChromeBrowserSessionFactory : IBrowserSessionFactory
    {
        private readonly IServiceProvider _services;

        public ChromeBrowserSessionFactory(IServiceProvider services)
        {
            _services = services;
        }

        public IBrowserSession CreateSession()
        {
            var session = ActivatorUtilities.CreateInstance<ChromeBrowserSession>(_services);

            session.Start();

            return session;
        }
    }
}
