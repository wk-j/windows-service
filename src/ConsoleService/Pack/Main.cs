using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/*
<add key="dotnet.myget.org" value="https://dotnet.myget.org/F/system-commandline/api/v3/index.json" />
*/

namespace ConsoleService {
    public class Main {

        class AppSettings { }

        static (AppSettings, IConfigurationRoot) LoadSettings() {
            var pathToExe = Assembly.GetExecutingAssembly().Location;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            var builder = new ConfigurationBuilder()
                 .SetBasePath(pathToContentRoot)
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var settings = new AppSettings();
            configuration.Bind(settings);

            return (settings, configuration);
        }

        public static async Task EntryPointAsync(string[] args) {
            var (settings, IConfigurationRoot) = LoadSettings();
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) => {
                    services.AddSingleton<AppSettings>(settings);
                    services.AddHostedService<ConsoleService.BackgroundService>();
                });

            if (isService) {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
                await builder.RunAsServiceAsync();
            } else {
                await builder.RunConsoleAsync();
            }
        }
    }
}