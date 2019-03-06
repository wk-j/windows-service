using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleService {
    public class BackgroundService : IHostedService, IDisposable {

        ILogger<BackgroundService> _logger;

        public BackgroundService(ILogger<BackgroundService> logger) {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("background service is starting.");

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("background service is stopping.");
            return Task.CompletedTask;
        }

        public void Dispose() {

        }
    }
}