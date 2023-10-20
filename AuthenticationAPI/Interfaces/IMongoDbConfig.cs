namespace AuthenticationAPI.Interfaces
{
    public interface IMongoDbConfig
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string UserCollectionName { get; set; }
    }
}
