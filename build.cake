#addin "wk.StartProcess"
#addin "wk.ProjectParser"

using PS = StartProcess.Processor;
using ProjectParser;

var nugetToken = EnvironmentVariable("npi");


var currentDir = new DirectoryInfo(".").FullName;
var publishDir = ".publish";
var version = DateTime.Now.ToString("yy.MM.dd.HHmm");

Task("Pack").Does(() => {
    CleanDirectory(publishDir);

    var settings = new DotNetCoreMSBuildSettings();
    settings.Properties["Version"] = new string[] { version };

    var app = new [] { "WindowsService", "ConsoleService" };
    foreach (var item in app) {
        DotNetCorePack($"src/{item}", new DotNetCorePackSettings {
            OutputDirectory = publishDir,
            MSBuildSettings = settings
        });
    }
});

Task("Publish-Web").Does(() => {
    CleanDirectory(publishDir);
    DotNetCorePublish($"tests/MyApi", new DotNetCorePublishSettings {
        Configuration = "Release",
        OutputDirectory = System.IO.Path.Combine(publishDir, "MyApi")
    });
});

Task("Publish-Console").Does(() => {
    CleanDirectory(publishDir);
    DotNetCorePublish($"tests/MyConcole", new DotNetCorePublishSettings {
        Configuration = "Release",
        OutputDirectory = System.IO.Path.Combine(publishDir, "MyConsole")
    });
});


Task("Publish-NuGet")
    .IsDependentOn("Pack")
    .Does(() => {
        var nupkg = new DirectoryInfo(publishDir).GetFiles("*.nupkg");
        foreach(var item in nupkg) {
            var package = item.FullName;
            try {
            NuGetPush(package, new NuGetPushSettings {
                Source = "https://www.nuget.org/api/v2/package",
                ApiKey = nugetToken
            });
            } catch (Exception ex) {
                Error(ex.Message);
            }
        }
});

Task("Install-Api")
    .IsDependentOn("Pack")
    .Does(() => {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var name = "WindowsService";
        var info = Parser.Parse($"src/{name}/{name}.csproj");

        PS.StartProcess($"dotnet tool uninstall -g {info.PackageId}");
        PS.StartProcess($"dotnet tool install   -g {info.PackageId}  --add-source {currentDir}/{publishDir} --version {info.Version}");
    });
Task("Install-Console")
    .IsDependentOn("Pack")
    .Does(() => {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        var name = "ConsoleService";
        var info = Parser.Parse($"src/{name}/{name}.csproj");

        PS.StartProcess($"dotnet tool uninstall -g {info.PackageId}");
        PS.StartProcess($"dotnet tool install   -g {info.PackageId}  --add-source {currentDir}/{publishDir} --version {version}");
    });

var target = Argument("target", "Pack");
RunTarget(target);