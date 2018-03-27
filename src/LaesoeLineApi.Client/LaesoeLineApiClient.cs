using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace LaesoeLineApi
{
    public class LaesoeLineApiClient : ILaesoeLineApiClient
    {
        public HttpClient HttpClient { get; }

        private static readonly RefitSettings _settings = new RefitSettings()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter(true) }
            }
        };

        public IAgentBookingApi AgentBooking => RestService.For<IAgentBookingApi>(HttpClient, _settings);
        public ICustomerBookingApi CustomerBooking => RestService.For<ICustomerBookingApi>(HttpClient, _settings);
        public IMyBookingApi MyBooking => RestService.For<IMyBookingApi>(HttpClient, _settings);
        public ITimetableApi Timetable => RestService.For<ITimetableApi>(HttpClient, _settings);

        public LaesoeLineApiClient(string baseUrl)
        {
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += '/';
            }

            HttpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public LaesoeLineApiClient(HttpClient client)
        {
            HttpClient = client;
        }

        public void SetAuthorization(string username, string password)
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
        }
    }
}
