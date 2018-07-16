#addin nuget:https://ci.appveyor.com/nuget/cake-monoapitools-ilf7s9bl0p9d/?package=Cake.MonoApiTools&version=1.0.0-preview11

var target = Argument("target", "Default");

var oldPackage = "https://ci.appveyor.com/api/buildjobs/7eahjee0x1tshgt1/artifacts/output%2FCake.MonoApiTools.0.0.0.1-preview5.nupkg";
var newPackage = "https://ci.appveyor.com/api/buildjobs/tib6m038i3ncjeih/artifacts/output%2FCake.MonoApiTools.1.0.0-preview11.nupkg";

Task("Default")
    .Does (() =>
{
    EnsureDirectoryExists("output");

    if (!FileExists("output/old.nupkg")) {
        DownloadFile(oldPackage, "output/old.nupkg");
        Unzip("output/old.nupkg", "output/old");
    }
    if (!FileExists("output/new.nupkg")) {
        DownloadFile(newPackage, "output/new.nupkg");
        Unzip("output/new.nupkg", "output/new");
    }

    Information("Running mono-api-info...");
    MonoApiInfo(
        "./output/old/lib/netstandard2.0/Cake.MonoApiTools.dll",
        "./output/old-info.xml",
        new MonoApiInfoSettings { SearchPaths = new [] { (DirectoryPath)"./tools/Cake" } });
    MonoApiInfo(
        "./output/new/lib/netstandard2.0/Cake.MonoApiTools.dll",
        "./output/new-info.xml",
        new MonoApiInfoSettings { SearchPaths = new [] { (DirectoryPath)"./tools/Cake" } });

    Information("Running mono-api-diff...");
    MonoApiDiff("./output/old-info.xml", "./output/new-info.xml", "./output/diff.xml");

    Information("Running mono-api-html...");
    MonoApiHtmlColorized("./output/old-info.xml", "./output/new-info.xml", "./output/diff.html");
    MonoApiMarkdownColorized("./output/old-info.xml", "./output/new-info.xml", "./output/diff.md");
});

RunTarget(target);
