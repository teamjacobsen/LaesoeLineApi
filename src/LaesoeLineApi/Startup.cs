using LaesoeLineApi.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LaesoeLineApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;

            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddTimetableFeature(Configuration.GetSection("Timetable"))
                .AddDistributedMemoryCache()
                .AddChromeSeleniumWebDriver(options => options.Headless = _hostingEnvironment.IsProduction() || false)
                .AddMvc()
                    .AddJsonOptions(options => options.SerializerSettings.Converters.Add(new CamelCaseEnumDictionaryKeyConverter()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
