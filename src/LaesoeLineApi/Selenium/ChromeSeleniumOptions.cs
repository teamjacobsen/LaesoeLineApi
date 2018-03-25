using Microsoft.Extensions.Options;

namespace LaesoeLineApi.Selenium
{
    public class ChromeSeleniumOptions : IOptions<ChromeSeleniumOptions>
    {
        public bool Headless { get; set; }

        ChromeSeleniumOptions IOptions<ChromeSeleniumOptions>.Value => this;
    }
}
