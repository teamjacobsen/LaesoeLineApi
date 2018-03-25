using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace LaesoeLineApi.Tests
{
    public class TestFixture
    {
        private const string CredentialsFilename = "credentials.json";

        public ILaesoeLineApiClient Api { get; private set; }

        public string AgentUsername { get; }
        public string AgentPassword { get; }
        public string CustomerUsername { get; }
        public string CustomerPassword { get; }

        public TestFixture()
        {
            Api = new LaesoeLineApiClient("http://localhost:51059/");

            if (File.Exists(CredentialsFilename))
            {
                using (var reader = new StreamReader(CredentialsFilename))
                {
                    var json = reader.ReadToEnd();

                    dynamic credentials = JsonConvert.DeserializeObject(json);

                    AgentUsername = credentials.Agent.Username;
                    AgentPassword = credentials.Agent.Password;
                    CustomerUsername = credentials.Customer.Username;
                    CustomerPassword = credentials.Customer.Password;
                }
            }
            else
            {
                Assert.True(false, "The file credentials.json must exist in order to run tests");
            }
        }
    }
}
