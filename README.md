## Windows Service

Install Windows Service boilerplate into specific Web API / Console App

[![Build Status](https://dev.azure.com/wk-j/windows-service/_apis/build/status/wk-j.windows-service?branchName=master)](https://dev.azure.com/wk-j/windows-service/_build/latest?definitionId=19&branchName=master)
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
    var isService = !(Debugger.IsAttached || args.Contains("--console"));
    var builder = new HostBuilder()
        .ConfigureServices((hostContext, services) => {
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
```

## Reference

- [Host ASP.NET Core in a Windows Service](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-2.2)
- [Running a .NET Core Generic Host App as a Windows Service](https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service)