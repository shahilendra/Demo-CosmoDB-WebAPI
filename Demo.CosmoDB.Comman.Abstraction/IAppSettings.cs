namespace Demo.CosmoDB.Comman.Abstraction
{
    public interface IAppSettings
    {
        string AppName { get; }
        string AppVersion { get; }
        string EndpointUri { get; }
        string PrimaryKey { get; }
        string DatabaseName { get; }
        int CacheExpiration { get; }
        string RedisConnection { get; }
        string Issuer { get; }
        string Audience { get; }
        string Key { get; }
    }
}