
using Amazon.Auth.AccessControlPolicy;
using MarketData.Common.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using System;
using System.Security.Cryptography;
using static MongoDB.Driver.WriteConcern;

namespace MarketData.DAL
{
    public class DbMarketRepository : IDbMarketRepository
    {
        private readonly ILogger<DbMarketRepository> _logger;
        public IDbContext Context { get; set; }
        
        public DbMarketRepository(ILogger<DbMarketRepository>  logger, IDbContext context) {
            Context= context;
            _logger = logger;
                
        }

        public async Task BulkUploadMarket(Guid jobId, MarketDataBase source,  IEnumerable<MarketDataFutureValue> Data)
        {
            await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataBase>(Context.MarketSourceCollectionName).InsertOneAsync(source);
            await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataFutureValue>(Context.MarketDataCollectionName).InsertManyAsync(Data);
        }

        public async Task<MarketDataContributionRequest> GetOrCreateMarketDataJob(Guid jobId, MarketDataContributionRequest? job = null)
        {
            var filterDefinition = Builders<MarketDataContributionRequest>.Filter.Eq("Id", jobId);

            var result = job;
            try
            {
                var queryResult = Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataContributionRequest>(Context.RequestCollectionName).Find(filterDefinition);

                if (queryResult != null && queryResult.Count() == 0  && job != null)
                {
                    await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataContributionRequest>(Context.RequestCollectionName).InsertOneAsync(job);
                }
                else if(queryResult != null && queryResult.Count() > 0 )
                {
                    result = await queryResult.Limit(1).SingleAsync();
                }
                else if (queryResult == null)
                    throw new Exception($"{jobId} not found and or No valid job supplied to be inserted");

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Job");
            }
            return result;
            
        }

        public async Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCode(string productCode)
        {
            var filterDefinition = Builders<MarketDataFutures>.Filter.Eq("ProductCode", productCode);
            var queryResult = await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataFutures>(Context.MarketSourceCollectionName).FindAsync(filterDefinition);
            var result = queryResult.ToEnumerable().SelectMany(s => s.Values);
            return result; //TODO : remove duplicates/average out duplicate values for tenors.
        }

        public async Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCodeAndUpdateTime(string productCode, DateTime updateTime)
        {
            var filterDefinitionProdCode = Builders<MarketDataFutures>.Filter.Eq("ProductCode", productCode) & 
                Builders<MarketDataFutures>.Filter.Gt("UpdateOn", updateTime);

            var queryResult = await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataFutures>(Context.MarketSourceCollectionName)
                .FindAsync(filterDefinitionProdCode);
            var result = queryResult.ToEnumerable().SelectMany(s => s.Values);
            return result; //TODO : remove duplicates/average out duplicate values for tenors.
        }

        public async Task SingleUploadMarket(Guid jobId, MarketDataFutures Data)
        {
            MarketDataBase source = (MarketDataBase)Data;
            await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataBase>(Context.MarketSourceCollectionName).InsertOneAsync(source);
            foreach (var val in Data.Values)
            {
                try
                {
                    await Context.DbServer.GetDatabase(Context.DatabaseName).GetCollection<MarketDataFutureValue>(Context.MarketDataCollectionName).InsertOneAsync(val);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Value upload failed for {val.Tenor}");
                    throw;
                }
            }
        }

        public Task UpdateJobStatus(Guid jobId, MarketDataJobStatus status = MarketDataJobStatus.Submitted)
        {
            throw new NotImplementedException();
        }

        
    }
}
