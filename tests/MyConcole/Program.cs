using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using ConsoleService;

namespace MyConcole {
    class Program {
        static async Task Main(string[] args) {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services.AddHostedService<ConsoleService.BackgroundService>();
                });

            if (isService) {
                await builder.RunAsServiceAsync();
            } else {
                await builder.RunConsoleAsync();
            }
        }
    }
}
