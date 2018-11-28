using System;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Daemon
{
    class Program
    {
        private static DaemonConfiguration DaemonConfiguration = new DaemonConfiguration();

        public static async Task Main(string[] args)
        {
            SetDaemonConfiguration();

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();

                    services.Configure<HostOptions>(hostOptions => hostOptions.ShutdownTimeout = DaemonConfiguration.GracefulShutdownPeriod);
                    services.Configure<DaemonConfiguration>(configuration =>
                    {
                        configuration.DaemonName = DaemonConfiguration.DaemonName;
                        configuration.GracefulShutdownPeriod = DaemonConfiguration.GracefulShutdownPeriod;
                    });

                    services.AddSingleton<IHostedService, DaemonService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConsole();
                }).Build();

            await host.RunAsync();
        }

        private static void SetDaemonConfiguration()
        {
            string gracefulShutownPeriodInSecondsFromEnv = Environment.GetEnvironmentVariable("GracefulShutdownPeriodInSeconds");
            if (!string.IsNullOrEmpty(gracefulShutownPeriodInSecondsFromEnv))
            {
               DaemonConfiguration.GracefulShutdownPeriod = TimeSpan.FromSeconds(long.Parse(gracefulShutownPeriodInSecondsFromEnv));
            }

            string daemonName = Environment.GetEnvironmentVariable("DaemonName");
            if (!string.IsNullOrEmpty(daemonName))
            {
                DaemonConfiguration.DaemonName = daemonName;
            }
        }
    }
}
