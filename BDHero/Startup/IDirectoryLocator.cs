namespace BDHero.Startup
{
    public interface IDirectoryLocator
    {
        bool IsPortable { get; }
        string InstallDir { get; }
        string ConfigDir { get; }
        string PluginDir { get; }
        string LogDir { get; }
    }
}