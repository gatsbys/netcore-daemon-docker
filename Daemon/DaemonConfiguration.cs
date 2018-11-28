using System;

namespace Daemon
{
    public class DaemonConfiguration
    {
        public string DaemonName { get; set; } = Guid.NewGuid().ToString();

        public TimeSpan GracefulShutdownPeriod { get; set; } = TimeSpan.FromSeconds(5);
    }
}