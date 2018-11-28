using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Daemon
{
    public class DaemonService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly DaemonConfiguration _daemonConfiguration;
        private readonly CancellationTokenSource _cancellationTokenSource;


        public DaemonService(
            ILogger<DaemonService> logger
            , IOptions<DaemonConfiguration> config)
        {
            _logger = logger;
            _daemonConfiguration = config.Value;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                $"Starting daemon: {_daemonConfiguration.DaemonName} " +
                $"with graceful shutdown of : {_daemonConfiguration.GracefulShutdownPeriod.Seconds} ");
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            _logger.LogInformation($"Trying stop Daemon gracefully for {_daemonConfiguration.GracefulShutdownPeriod.Seconds} second(s).");
            Thread.Sleep(TimeSpan.FromSeconds(7));
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning($"The graceful period timeout was set to {_daemonConfiguration.GracefulShutdownPeriod.Seconds} but the shutdown has been taken more than this time");
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
