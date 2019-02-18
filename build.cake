#addin "wk.StartProcess"
#addin "wk.ProjectParser"

using PS = StartProcess.Processor;
using ProjectParser;

var nugetToken = EnvironmentVariable("npi");
var name = "WindowsService";

var currentDir = new DirectoryInfo(".").FullName;
var info = Parser.Parse($"src/{name}/{name}.csproj");
var publishDir = ".publish";

Task("Pack").Does(() => {
    CleanDirectory(publishDir);
    DotNetCorePack($"src/{name}", new DotNetCorePackSettings {
        OutputDirectory = publishDir
    });
});

Task("Publish-Web").Does(() => {
    CleanDirectory(publishDir);
    DotNetCorePublish($"src/MyApi", new DotNetCorePublishSettings {
        Configuration = "Release",
        OutputDirectory = System.IO.Path.Combine(publishDir, name)
    });
});

Task("Publish-NuGet")
    .IsDependentOn("Pack")
    .Does(() => {
        var nupkg = new DirectoryInfo(publishDir).GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = nugetToken
        });
});

Task("Install")
    .IsDependentOn("Pack")
    .Does(() => {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        PS.StartProcess($"dotnet tool uninstall -g {info.PackageId}");
        PS.StartProcess($"dotnet tool install   -g {info.PackageId}  --add-source {currentDir}/{publishDir} --version {info.Version}");
    });

var target = Argument("target", "Pack");
RunTarget(target);