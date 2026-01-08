using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VersionCheck
{
    public sealed class VersionChecker : IVersionChecker
    {
        private readonly HttpClient _http;
        private readonly VersionCheckerOptions _options;

        public VersionChecker(HttpClient http, VersionCheckerOptions options)
        {
            _http = http;
            _options = options;
        }

        public async Task<(bool HasUpdate, string Message)> CheckAsync()
        {
            var localVersion = Assembly.GetEntryAssembly()?.GetName().Version;
            if (localVersion is null)
            {
                return (false, "Local version unknown.");
            }

            var remote = await _http.GetFromJsonAsync<VersionManifest>(_options.VersionManifestUrl);
            if (remote is null)
            {
                return (false, "Invalid version manifest.");
            }

            var remoteVersion = Version.Parse(remote.Version);
            return remoteVersion > localVersion
                ? (true, $"New version {remoteVersion} available.")
                : (false, "App is up to date.");
        }

        private sealed record VersionManifest(string Version);
    }
}
