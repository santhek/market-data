using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Common.Models
{
    public abstract record MarketDataBase
    {
        public MarketDataBase(string publisher, string productCode, string unit) { 
            Publisher = publisher;
            ProductCode = productCode;
            Unit = unit;
        }
        public string? Publisher { get; set; }
        public string? ProductCode { get; set; }
        public DateTime UpdateOn { get; init; } = DateTime.Now;
        public string? Unit { get; set; }

        

    }
}
