using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LaesoeLineApi
{
    public abstract class HostedService : IHostedService
    {
        private CancellationTokenSource _cts;
        private Task _runningTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _runningTask = RunAsync(_cts.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();

            await Task.WhenAny(_runningTask, Task.Delay(Timeout.Infinite, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
        }

        protected abstract Task RunAsync(CancellationToken cancellationToken);
    }
}
