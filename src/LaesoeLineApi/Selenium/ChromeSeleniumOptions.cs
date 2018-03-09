using Microsoft.Extensions.Options;

namespace LaesoeLineApi.Selenium
{
    public class ChromeSeleniumOptions : IOptions<ChromeSeleniumOptions>
    {
        public bool Headless { get; set; }
        public int Port { get; set; } = 5500;

        ChromeSeleniumOptions IOptions<ChromeSeleniumOptions>.Value => this;
    }
}
