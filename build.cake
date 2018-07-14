#addin "Cake.FileHelpers"

#tool xunit.runner.console&version=2.4.0-rc.2.build4045

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var version = "0.0.0.1-preview5";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Externals")
    .Does (() =>
{
    Information("Downloading the mono-api-tools package...");

    var url = "https://xamjenkinsartifact.blob.core.windows.net/components-opensource-pr-public-artifacts/ArtifactsFor-31/ab23e6575207a4726f2932f89247f4ede8b07023/XPlat/Mono.ApiTools/output/signed/Mono.ApiTools.5.12.0.273.nupkg";
    var dest = "./externals/Mono.ApiTools.nupkg";

    if (!FileExists(dest)) {
        EnsureDirectoryExists("./externals/");
        CleanDirectories("./externals/");
        DownloadFile(url, dest);
        Unzip(dest, "./externals/");
    }

    Information("Download complete.");
});

Task("Build")
    .IsDependentOn("Externals")
    .Does (() =>
{
    var sln = "./src/Cake.MonoApiTools.sln";

    Information("Building {0}...", sln);

    var settings = new MSBuildSettings()
        .SetConfiguration(configuration)
        .WithRestore()
        .WithProperty("NoWarn", "1591") // ignore missing XML doc warnings
        .WithProperty("TreatWarningsAsErrors", "True")
        .SetVerbosity(Verbosity.Minimal)
        .SetNodeReuse(false);

    MSBuild(sln, settings);

    Information("Build complete.");
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    var test = $"./src/Cake.MonoApiTools.Tests/bin/{configuration}/net462/Cake.MonoApiTools.Tests.dll";

    Information("Testing {0}...", test);

    XUnit2(test, new XUnit2Settings {
        OutputDirectory = "./output/",
        XmlReport = true
    });

    Information("Test complete.");
});

Task("Package")
    .IsDependentOn("Build")
    .Does (() =>
{
    var proj = "./src/Cake.MonoApiTools/Cake.MonoApiTools.csproj";

    Information("Packing {0}...", proj);

    var settings = new MSBuildSettings()
        .SetConfiguration(configuration)
        .WithTarget("Pack")
        .WithProperty("IncludeSymbols", "True")
        .WithProperty("PackageVersion", version)
        .WithProperty("PackageOutputPath", MakeAbsolute((DirectoryPath)"./output/").FullPath);

    MSBuild (proj, settings);

    Information("Pack complete.");
});

Task("Default")
    .IsDependentOn("Externals")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
