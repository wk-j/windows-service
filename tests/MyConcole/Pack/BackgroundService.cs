using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleService {
    public class BackgroundService : IHostedService, IDisposable {
        private Timer _timer;

        public BackgroundService() {
        }

        public Task StartAsync(CancellationToken cancellationToken) {
            Console.WriteLine("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state) {
            Console.WriteLine("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            Console.WriteLine("Timed Background Service is stopping.2");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose() {
            _timer?.Dispose();
        }
    }
}