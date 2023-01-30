using MongoDB.Driver;


namespace MarketData.DAL
{
    public interface IDbContext
    {
        public MongoClient DbServer { get; init; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string RequestCollectionName { get; set; }
        public string MarketDataCollectionName { get; set; }
        public string MarketSourceCollectionName { get; set; }
    }
}
