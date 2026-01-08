namespace VersionCheck
{
    public interface IVersionChecker
    {
        Task<(bool HasUpdate, string Message)> CheckAsync();

    }
}
