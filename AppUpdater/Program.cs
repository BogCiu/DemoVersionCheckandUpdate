using System.Diagnostics;
using System.IO.Compression;

var targetDir = args[0];
var updateZip = args[1];
var pid = int.Parse(args[2]);

// Wait for app to exit
Process.GetProcessById(pid).WaitForExit();

// Extract update
var extractDir = Path.Combine(
    Path.GetTempPath(),
    "app_update_extract"
);

if (Directory.Exists(extractDir))
    Directory.Delete(extractDir, true);

ZipFile.ExtractToDirectory(updateZip, extractDir);

// Replace files
foreach (var file in Directory.GetFiles(extractDir))
{
    var dest = Path.Combine(
        targetDir,
        Path.GetFileName(file)
    );

    File.Copy(file, dest, true);
}

// Restart app
var exe = Directory
    .GetFiles(targetDir, "*.exe")
    .First(f => !f.EndsWith("AppUpdater.exe"));

Process.Start(exe);