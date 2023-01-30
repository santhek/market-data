

using MongoDB.Bson.Serialization.Attributes;

namespace MarketData.Common.Models
{
    [BsonIgnoreExtraElements]
    public record MarketDataFutureValue
    {
        public string Tenor { get; set; }
        public decimal Last { get; set; }
        public decimal Open{ get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public DateTime PublishTime { get; set; }

    }
}
