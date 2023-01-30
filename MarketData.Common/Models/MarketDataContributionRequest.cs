namespace MarketData.Common.Models
{
    public class MarketDataContributionRequest
    {
        public DateTime CreatedOn { get; init; } = DateTime.Now;
        public Guid Id { get; init; } = Guid.NewGuid();
        public MarketDataJobStatus JobStatus { get; set; } = MarketDataJobStatus.Submitted;
        public MarketDataFutures? Data { get; set; }
    }

    public enum MarketDataJobStatus
    {
        Submitted, Started, Pending, Success, Error
    }
}
