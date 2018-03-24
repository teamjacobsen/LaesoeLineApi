using Newtonsoft.Json;
using System.IO;

namespace LaesoeLineApi.Tests
{
    public class TestFixture
    {
        public ILaesoeLineApiClient Api { get; private set; }

        public string AgentUsername { get; }
        public string AgentPassword { get; }
        public string CustomerUsername { get; }
        public string CustomerPassword { get; }

        public TestFixture()
        {
            Api = new LaesoeLineApiClient("http://localhost:51059/");

            using (var reader = new StreamReader("credentials.json"))
            {
                var json = reader.ReadToEnd();

                dynamic credentials = JsonConvert.DeserializeObject(json);

                AgentUsername = credentials.Agent.Username;
                AgentPassword = credentials.Agent.Password;
                CustomerUsername = credentials.Customer.Username;
                CustomerPassword = credentials.Customer.Password;
            }
        }
    }
}
