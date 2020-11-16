var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");

var packageNameWorker = "CronBackgroundServices";
var packageNameEndpoints = "Slackbot.Net.Endpoints";
var packageNameHttpClient = "Slackbot.Net.SlackClients.Http";
var packageNameShared = "Slackbot.Net.Shared";

private string ProjectPath(string name){
    return $"./source/src/{name}/{name}.csproj";
}

var version = "4.1.0";
var outputDir = "./output";

Task("Build")
    .Does(() => {
        DotNetCoreBuild("./source/Slackbot.Net.sln", new DotNetCoreBuildSettings { Configuration = "Release" });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        Pack(packageNameWorker);
        Pack(packageNameEndpoints);
        Pack(packageNameHttpClient);
        Pack(packageNameShared);
  
});

private void Pack(string proj){
    var coresettings = new DotNetCorePackSettings
    {
        Configuration = "Release",
        OutputDirectory = outputDir,
    };
    coresettings.MSBuildSettings = new DotNetCoreMSBuildSettings()
                                    .WithProperty("Version", new[] { version });

    DotNetCorePack(ProjectPath(proj), coresettings);
}

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = Argument("nugetapikey", "must-be-given")
        };

        DotNetCoreNuGetPush($"{outputDir}/{packageNameWorker}.{version}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{packageNameEndpoints}.{version}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{packageNameHttpClient}.{version}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{packageNameShared}.{version}.nupkg", settings);

});

RunTarget(target);