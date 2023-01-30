using MarketData.Common.Models;


namespace MarketData.DAL
{
    public interface IDbMarketRepository
    {
        Task BulkUploadMarket(Guid jobId, MarketDataBase source, IEnumerable<MarketDataFutureValue> Data);

        Task SingleUploadMarket(Guid jobId, MarketDataFutures Data);

        Task UpdateJobStatus(Guid jobId, MarketDataJobStatus status = MarketDataJobStatus.Submitted);

        Task<MarketDataContributionRequest> GetOrCreateMarketDataJob(Guid jobId, MarketDataContributionRequest? job = null);

        Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCode(string productCode);

        Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCodeAndUpdateTime(string productCode, DateTime updateTime);
    }
}
