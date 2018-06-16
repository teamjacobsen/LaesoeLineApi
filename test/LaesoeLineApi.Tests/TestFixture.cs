using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace LaesoeLineApi.Tests
{
    public class TestFixture
    {
        private readonly Dictionary<string, Credentials> _credentials;

        private const string CredentialsFilename = "credentials.json";

        public ILaesoeLineApiClient Api { get; private set; }

        public TestFixture()
        {
            Api = new LaesoeLineApiClient("http://localhost:51059/");

            if (File.Exists(CredentialsFilename))
            {
                using (var reader = new StreamReader(CredentialsFilename))
                {
                    var json = reader.ReadToEnd();

                    _credentials = JsonConvert.DeserializeObject<Dictionary<string, Credentials>>(json);
                }
            }
            else
            {
                Assert.True(false, "The file credentials.json must exist in order to run tests");
            }
        }

        public Credentials GetCredentials(string name)
        {
            return _credentials[name];
        }

        public class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
