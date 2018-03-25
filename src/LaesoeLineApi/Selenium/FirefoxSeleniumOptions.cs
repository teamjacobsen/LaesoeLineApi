using Microsoft.Extensions.Options;

namespace LaesoeLineApi.Selenium
{
    public class FirefoxSeleniumOptions : IOptions<FirefoxSeleniumOptions>
    {
        public bool Headless { get; set; }

        FirefoxSeleniumOptions IOptions<FirefoxSeleniumOptions>.Value => this;
    }
}
