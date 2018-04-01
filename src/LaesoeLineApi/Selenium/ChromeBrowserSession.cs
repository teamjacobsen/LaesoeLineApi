using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi.Selenium
{
    public class ChromeBrowserSession : IBrowserSession
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ChromeBrowserSession> _logger;
        private readonly IOptions<ChromeSeleniumOptions> _options;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim _wait = new SemaphoreSlim(0);
        private readonly ConcurrentQueue<QueueItem> _queue = new ConcurrentQueue<QueueItem>();
        private readonly Thread _thread;

        public ChromeBrowserSession(IServiceProvider services, ILogger<ChromeBrowserSession> logger, IOptions<ChromeSeleniumOptions> options)
        {
            _services = services;
            _logger = logger;
            _options = options;
            _thread = new Thread(Run);
        }

        public void Start() => _thread.Start();

        public async Task GoToAsync(string url)
        {
            await InvokeAsync(driver =>
            {
                var stopwatch = Stopwatch.StartNew();

                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

                // https://stackoverflow.com/questions/47877899/does-chrome-chrome-driver-support-pageloadstrategy-eager-for-selenium
                // https://stackoverflow.com/questions/11001030/how-i-can-check-whether-a-page-is-loaded-completely-or-not-in-web-driver
                var ok = false;
                for (var i = 0; i < 5; i++)
                {
                    try
                    {
                        driver.Url = url;

                        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                        wait.Until(_ => (bool?)driver.ExecuteScript("return (document.readyState === 'interactive' || document.readyState === 'complete') && window.jQuery && window.jQuery.active === 0") == true);
                        Thread.Sleep(500);

                        ok = true;
                        break;
                    }
                    catch (WebDriverTimeoutException e)
                    {
                        _logger.LogWarning(e, "Timeout while navigating to {Url}", url);
                    }
                }

                if (!ok)
                {
                    throw new ApiException(ApiStatus.GatewayTimeout);
                }

                stopwatch.Stop();

                _logger.LogInformation("Navigation to {Url} took {ElapsedMilliseconds}ms", url, stopwatch.ElapsedMilliseconds);

            });
        }

        public async Task<TPage> GoToAsync<TPage>() where TPage : IPage
        {
            var page = ActivatorUtilities.CreateInstance<TPage>(_services, this);

            await GoToAsync(page.Url);

            return page;
        }

        public async Task InvokeAsync(Action<RemoteWebDriver> handler)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            _queue.Enqueue(new QueueItem()
            {
                TaskCompletionSource = tcs,
                Handler = driver =>
                {
                    handler(driver);
                    return null;
                }
            });

            _wait.Release();

            try
            {
                await tcs.Task;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
        }

        public async Task<TResult> InvokeAsync<TResult>(Func<RemoteWebDriver, TResult> handler)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            _queue.Enqueue(new QueueItem()
            {
                TaskCompletionSource = tcs,
                Handler = driver => handler(driver)
            });

            _wait.Release();

            try
            {
                var result = await tcs.Task;

                return (TResult)result;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
        }

        public void Run()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            var chromeOptions = new ChromeOptions();

            if (_options.Value.Incognito)
            {
                chromeOptions.AddArgument("--incognito");
            }

            if (_options.Value.Headless)
            {
                chromeOptions.AddArgument("--headless");

                // https://developers.google.com/web/updates/2017/04/headless-chrome
                // https://bugs.chromium.org/p/chromium/issues/detail?id=737678
                chromeOptions.AddArgument("--disable-gpu");
            }

            // https://stackoverflow.com/questions/47877899/does-chrome-chrome-driver-support-pageloadstrategy-eager-for-selenium
            chromeOptions.PageLoadStrategy = PageLoadStrategy.None; // Eager is not supported

            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions))
            {
                // https://github.com/SeleniumHQ/selenium/issues/4988
                FixDriverCommandExecutionDelay(driver);

                _logger.LogInformation("Chrome driver is ready to serve");

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _wait.Wait(cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }

                    try
                    {
                        if (_queue.TryDequeue(out var item))
                        {
                            try
                            {
                                var result = item.Handler(driver);

                                cancellationToken.ThrowIfCancellationRequested();

                                item.TaskCompletionSource.SetResult(result);
                            }
                            catch (Exception e)
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                item.TaskCompletionSource.SetException(e);
                            }
                        }
                    }
                    finally
                    {
                        _wait.Release();
                    }
                }

                _logger.LogInformation("Exiting chrome driver thread");
            }
        }

        // https://github.com/dotnet/corefx/issues/24104
        // https://github.com/atata-framework/atata/issues/101
        // https://github.com/atata-framework/atata/blob/17187282d2ddd356c0195802253513428d3087b4/src/Atata/Context/DriverAtataContextBuilder%601.cs#L65-L107
        private static void FixDriverCommandExecutionDelay(RemoteWebDriver driver)
        {
            PropertyInfo commandExecutorProperty = typeof(RemoteWebDriver).GetProperty("CommandExecutor", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

            if (commandExecutorProperty == null)
            {
                throw new MissingMemberException(typeof(RemoteWebDriver).FullName, "CommandExecutor");
            }

            ICommandExecutor commandExecutor = (ICommandExecutor)commandExecutorProperty.GetValue(driver);

            FieldInfo GetRemoteServerUriField(ICommandExecutor executor)
            {
                return executor.GetType().GetField("remoteServerUri", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);
            }

            FieldInfo remoteServerUriField = GetRemoteServerUriField(commandExecutor);

            if (remoteServerUriField == null)
            {
                FieldInfo internalExecutorField = commandExecutor.GetType().GetField("internalExecutor", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
                commandExecutor = (ICommandExecutor)internalExecutorField.GetValue(commandExecutor);
                remoteServerUriField = GetRemoteServerUriField(commandExecutor);
            }

            if (remoteServerUriField != null)
            {
                string remoteServerUri = remoteServerUriField.GetValue(commandExecutor).ToString();

                string localhostUriPrefix = "http://localhost";

                if (remoteServerUri.StartsWith(localhostUriPrefix))
                {
                    remoteServerUri = remoteServerUri.Replace(localhostUriPrefix, "http://127.0.0.1");

                    remoteServerUriField.SetValue(commandExecutor, new Uri(remoteServerUri));
                    return;
                }
            }

            // https://github.com/SeleniumHQ/selenium/issues/4988
            throw new Exception("Unable to apply fix for issue 4988");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private class QueueItem
        {
            public TaskCompletionSource<object> TaskCompletionSource { get; set; }
            public Func<RemoteWebDriver, object> Handler { get; set; }
        }
    }
}
