using Demo.CosmoDB.Comman.Abstraction;

namespace Demo.CosmoDB.Comman
{
    public class AppSettings : IAppSettings
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set;}
    }
}