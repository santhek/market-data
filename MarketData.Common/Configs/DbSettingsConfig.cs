using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Common.Configs
{
    public class DbSettingsConfig
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string RequestCollectionName { get; set; }
        public string MarketDataCollectionName { get; set; }
        public string MarketSourceCollectionName { get; set; }

    }
}
