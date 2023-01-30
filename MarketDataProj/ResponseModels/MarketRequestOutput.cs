using MarketData.Common.Models;

namespace MarketDataProj.ResponseModels
{
    public class MarketRequestOutput
    {
        public MarketRequestOutput() { }
        public MarketDataContributionRequest Request { get; set; }

        public string Error { get; set; }
    }
}