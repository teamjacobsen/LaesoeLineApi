using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Refit;
using System.Net.Http;

namespace ExampleClient.Api
{
    public class LaesoeLineApi
    {
        private readonly HttpClient _client;

        private static readonly RefitSettings _settings = new RefitSettings()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter(true) }
            }
        };

        public IAgentBookingApi AgentBooking => RestService.For<IAgentBookingApi>(_client, _settings);
        public ICustomerBookingApi CustomerBooking => RestService.For<ICustomerBookingApi>(_client, _settings);
        public IMyBookingApi MyBooking => RestService.For<IMyBookingApi>(_client, _settings);
        public ITimetableApi Timetable => RestService.For<ITimetableApi>(_client, _settings);

        public LaesoeLineApi(HttpClient client) => _client = client;
    }
}
