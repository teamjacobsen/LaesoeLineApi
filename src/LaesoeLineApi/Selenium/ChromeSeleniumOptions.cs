using Microsoft.Extensions.Options;

namespace LaesoeLineApi.Selenium
{
    public class ChromeSeleniumOptions : IOptions<ChromeSeleniumOptions>
    {
        public bool Headless { get; set; }
        public int Port { get; set; } = 4444;

        ChromeSeleniumOptions IOptions<ChromeSeleniumOptions>.Value => this;
    }
}
