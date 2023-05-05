using Demo.CosmoDB.Comman.Abstraction;

namespace Demo.CosmoDB.Comman
{
    public class AppSettings : IAppSettings
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }
        public int CacheExpiration { get; set; }
        public string RedisConnection { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}