using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;

var targetDir = args[0];
var updateZip = args[1];
try
{
    var pid = int.Parse(args[2]);

    // Wait for app to exit
    Process.GetProcessById(pid).WaitForExit();
} 
catch (ArgumentException ae)
{
    if(ae.Message.Contains("Process with an Id of") && ae.Message.Contains("is not running."))
    {
        Console.WriteLine("Process is not running, proceeding with update.");
    }
    else
    {
        throw;
    }
}
// Create a temporary directory for downloading and extracting the update


// Temporarily Download new ZIP files from the manifest url

// Extract the ZIP file to a temporary directory
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

// Delete the temporary ZIP

// Restart app
var exe = Directory
    .GetFiles(targetDir, "*.exe")
    .First(f => !f.EndsWith("AppUpdater.exe"));

Process.Start(exe);