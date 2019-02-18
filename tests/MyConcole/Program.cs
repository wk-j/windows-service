using System;

namespace MyConcole {
    class Program {
        static async Task Main(string[] args) {
            await new HostBuilder().ConfigureServices((hostContext, services) => {
                services.AddHostedService<ConsoleService>();
            })
            .RunConsoleAsync();
        }
    }
}
