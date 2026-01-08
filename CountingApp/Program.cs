using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using VersionCheck;

var services = new ServiceCollection();

// Register version checker
services.AddHttpClient<IVersionChecker, VersionChecker>();
services.AddSingleton(new VersionCheckerOptions
{
    VersionManifestUrl =
        "https://raw.githubusercontent.com/BogCiu/DemoVersionCheckandUpdate/main/version.json"
});


var provider = services.BuildServiceProvider();

var checker = provider.GetRequiredService<IVersionChecker>();

var (hasUpdate, message) = await checker.CheckAsync();

Console.WriteLine(message);

if (hasUpdate)
{
    LaunchUpdater();
    return;
}

RunApp();

static void RunApp()
{
    for (int i = 1; i <= 10; i++)
    {
        Console.WriteLine(i);
        Thread.Sleep(500);
    }
}

static void LaunchUpdater()
{
    Process.Start("AppUpdater.exe", new[]
    {
        AppContext.BaseDirectory,
        "update.zip",
        Process.GetCurrentProcess().Id.ToString()
    });

    Environment.Exit(0);
}
