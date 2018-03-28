using Microsoft.Extensions.Options;

namespace LaesoeLineApi.Selenium
{
    public class ChromeSeleniumOptions : IOptions<ChromeSeleniumOptions>
    {
        public bool Incognito { get; set; } = true;

        public bool Headless { get; set; } = false;

        ChromeSeleniumOptions IOptions<ChromeSeleniumOptions>.Value => this;
    }
}
