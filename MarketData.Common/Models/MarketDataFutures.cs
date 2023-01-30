using MongoDB.Bson.Serialization.Attributes;


namespace MarketData.Common.Models
{
    [BsonIgnoreExtraElements]
    public record MarketDataFutures : MarketDataBase
    {
        public MarketDataFutures(string publisher,string productCode,string unit, IList<MarketDataFutureValue> values) : base(publisher, productCode, unit)
        {
            Values = values ?? new List<MarketDataFutureValue>();
        }
        

        public IList<MarketDataFutureValue> Values { get; set; }
    }
}
