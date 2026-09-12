using Bullseye;
using static Bullseye.Targets;
using static SimpleExec.Command;

const string solution = "source/Slackbot.Net.sln";
const string releasesDir = "releases";

const string Restore = "restore";
const string Build = "build";
const string Test = "test";
const string Pack = "pack";
const string Publish = "publish";

Target(Restore, () => RunAsync("dotnet", $"restore {solution}"));

Target(Build, new[] { Restore }, () => RunAsync("dotnet", $"build {solution} --no-restore"));

Target(Test, new[] { Build }, () => RunAsync("dotnet", $"test {solution} --no-build"));

Target(Pack, new[] { Build }, async () =>
{
    var version = Environment.GetEnvironmentVariable("BUILD_VERSION");
    var informationalVersion = Environment.GetEnvironmentVariable("BUILD_INFORMATIONAL_VERSION");
    var releaseNotes = Environment.GetEnvironmentVariable("BUILD_RELEASE_NOTES");

    var properties = "";
    if (!string.IsNullOrWhiteSpace(version))
        properties += $" /p:Version={version}";
    if (!string.IsNullOrWhiteSpace(informationalVersion))
        properties += $" /p:InformationalVersion={informationalVersion}";
    if (!string.IsNullOrWhiteSpace(releaseNotes))
        properties += $" /p:PackageReleaseNotes=\"{releaseNotes}\"";

    await RunAsync("dotnet", $"pack {solution} -c Release -o {releasesDir}{properties}");
});

Target(Publish, new[] { Pack }, async () =>
{
    var apiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY")
        ?? throw new InvalidOperationException("NUGET_API_KEY environment variable is required for publish");

    foreach (var package in Directory.GetFiles(releasesDir, "*.nupkg", SearchOption.AllDirectories))
        await RunAsync("dotnet", $"nuget push \"{package}\" -k {apiKey} -s https://api.nuget.org/v3/index.json --skip-duplicate");
});

Target("default", new[] { Test });

await RunTargetsAndExitAsync(args);
