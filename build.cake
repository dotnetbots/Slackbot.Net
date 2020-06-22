var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");

var packageNameWorker = "Slackbot.Net";
var packageNameEndpoints = "Slackbot.Net.Endpoints";

var packageNameHttpClient = "Slackbot.Net.SlackClients.Http";
var packageNameRtmClient = "Slackbot.Net.SlackClients.Rtm";

var packageNameAbstractionsHandlers = "Slackbot.Net.Abstractions.Handlers";
var packageNameAbstractionsHosting = "Slackbot.Net.Abstractions.Hosting";
var packageNameAbstractionsPublishers = "Slackbot.Net.Abstractions.Publishers";

var packageNamePublishersLogger = "Slackbot.Net.Extensions.Publishers.Logger";
var packageNamePublishersSlack = "Slackbot.Net.Extensions.Publishers.Slack";


private string ProjectPath(string name){
    return $"./source/src/{name}/{name}.csproj";
}

var version = "3.0.0-preview052";
var outputDir = "./output";

Task("Build")
    .Does(() => {
        DotNetCoreBuild("./source/Slackbot.Net.sln", new DotNetCoreBuildSettings { Configuration = "Release" });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest("./source/test/Slackbot.Net.SlackClients.Rtm.Tests.Unit", new DotNetCoreTestSettings());
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        Pack(packageNameWorker);
        Pack(packageNameEndpoints);

        Pack(packageNameHttpClient);
        Pack(packageNameRtmClient);
        
        Pack(packageNameAbstractionsHandlers);
        Pack(packageNameAbstractionsHosting);
        Pack(packageNameAbstractionsPublishers);

        Pack(packageNamePublishersLogger);
        Pack(packageNamePublishersSlack);
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
        DotNetCoreNuGetPush($"{outputDir}/{packageNameRtmClient}.{version}.nupkg", settings);


        DotNetCoreNuGetPush($"{outputDir}/{packageNameAbstractionsHandlers}.{version}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{packageNameAbstractionsHosting}.{version}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{packageNameAbstractionsPublishers}.{version}.nupkg", settings);

        DotNetCoreNuGetPush($"{outputDir}/{packageNamePublishersLogger}.{version}.nupkg", settings);
        DotNetCoreNuGetPush($"{outputDir}/{packageNamePublishersSlack}.{version}.nupkg", settings);

});

RunTarget(target);