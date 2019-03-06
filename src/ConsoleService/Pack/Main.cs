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

        static string GetContentRoot() {
            var pathToExe = Assembly.GetExecutingAssembly().Location;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            return pathToContentRoot;
        }

        static (AppSettings, IConfigurationRoot) LoadSettings() {
            var pathToContentRoot = GetContentRoot();
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
                var pathToContentRoot = GetContentRoot();
                Directory.SetCurrentDirectory(pathToContentRoot);
                await builder.RunAsServiceAsync();
            } else {
                await builder.RunConsoleAsync();
            }
        }
    }
}