## Windows Service

Install Windows Service boilerplate into specific Web API / Console App

[![NuGet](https://img.shields.io/nuget/v/wk.WindowsService.svg)](https://www.nuget.org/packages/wk.WindowsService)
[![NuGet](https://img.shields.io/nuget/v/wk.ConsoleService.svg)](https://www.nuget.org/packages/wk.ConsoleService)

## Installation

```bash
dotnet tool install -g wk.WindowsService
dotnet tool install -g wk.ConsoleService
```

## Usage

Install template

```bash
wk-windows-service <APP-PATH>
wk-console-service <APP-PATH>
```

Web API

```csharp
public static void Main(string[] args) {
    var isService = !(Debugger.IsAttached || args.Contains("--console"));
    if (isService) {
        var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
        var pathToContentRoot = Path.GetDirectoryName(pathToExe);
        Directory.SetCurrentDirectory(pathToContentRoot);

        var newArgs = args.Where(x => x != "--console").ToArray();
        CreateWebHostBuilder(newArgs).Build().RunAsCustomService();
    } else {
        CreateWebHostBuilder(args).Build().Run();
    }
}
```

Console App

```csharp
static async Task Main(string[] args) {
    await new HostBuilder()
        .ConfigureServices((hostContext, services) => {
            services.AddHostedService<ConsoleService>();
        })
    .RunConsoleAsync();
}
```