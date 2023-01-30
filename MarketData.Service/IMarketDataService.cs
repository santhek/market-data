using MarketData.Common.Models;
using System.Collections.Concurrent;

namespace MarketData.Service
{
    public interface IMarketDataService
    {
        public ConcurrentQueue<MarketDataContributionRequest> Jobs { get; set; }
        public Task<MarketDataContributionRequest> AddToQueue(MarketDataContributionRequest job);
        public Task<MarketDataContributionRequest> GetStatusOrCreateJobAsync(Guid jobId, MarketDataContributionRequest? marketDataJob = null);
        public Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCodeAsync(string productCode);
        public Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCodeAndUpdateTimeAsync(string productCode, DateTime updateTime);
    }
}