namespace Demo.CosmoDB.Comman.Abstraction
{
    public interface IAppSettings
    {
        string AppName { get; }
        string AppVersion { get; }
        string EndpointUri { get; }
        string PrimaryKey { get; }
        string DatabaseName { get;}
    }
}