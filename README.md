## Windows Service

Install Windows Service boilerplate into specific Web API

[![NuGet](https://img.shields.io/nuget/v/wk.WindowsService.svg)](https://www.nuget.org/packages/wk.WindowsService)

## Installation

```bash
dotnet tool install -g wk.WindowsService
```

## Usage

Install template

```bash
wk-windows-service <APP-PATH>
```

Update Program.cs

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