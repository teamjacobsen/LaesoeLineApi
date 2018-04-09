using idunno.Authentication;
using LaesoeLineApi.Converters;
using LaesoeLineApi.Filters;
using LaesoeLineApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

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
                .AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("basic", new BasicAuthScheme() { Type = "basic", Description = "The Agent or Customer login" });
                    options.DocumentFilter<BasicAuthFilter>();
                    options.SwaggerDoc("v1", new Info() { Title = "Laesoe Line", Version = "v1" });
                    options.DescribeAllEnumsAsStrings();
                    options.CustomSchemaIds(x => x.FullName.Replace("LaesoeLineApi.Features.", string.Empty));
                    options.OperationFilter<HonorRequiredAttributeFilter>();

                    var xmlPath = Path.Combine(AppContext.BaseDirectory, "LaesoeLineApi.xml");
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                });

            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasic(options => {
                    options.AllowInsecureProtocol = _hostingEnvironment.IsDevelopment();

                    options.Events.OnValidateCredentials = context =>
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String, context.Options.ClaimsIssuer),
                            new Claim(ClaimTypes.Authentication, context.Password, ClaimValueTypes.String, context.Options.ClaimsIssuer)
                        };

                        context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));

                        context.Success();
                        return Task.CompletedTask;
                    };
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new VehicleValueConverter());
                    options.SerializerSettings.Converters.Add(new VehicleDictionaryKeyConverter());
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseCors(options => options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader())
                .UseAuthentication()
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Laesoe Line");
                });
        }
    }
}
