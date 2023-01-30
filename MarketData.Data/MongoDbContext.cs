using MarketData.DAL;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MarketData.Common.Configs;

namespace MarketData.Data
{
    public class MongoDbContext : IDbContext
    {
        
        public MongoDbContext(IOptions<DbSettingsConfig> config) 
        {
            ConnectionString = config.Value.ConnectionString;
            DatabaseName = config.Value.DatabaseName;
            RequestCollectionName = config.Value.RequestCollectionName;
            MarketDataCollectionName = config.Value.MarketDataCollectionName;
            MarketSourceCollectionName = config.Value.MarketSourceCollectionName;
            DbServer = new MongoClient(ConnectionString);
        }

        public MongoClient DbServer { get; init; }
        public string ConnectionString { get ; set ; }
        public string DatabaseName { get ; set ; }
        public string RequestCollectionName { get; set; }
        public string MarketDataCollectionName { get; set; }
        public string MarketSourceCollectionName { get; set; }
    }
}