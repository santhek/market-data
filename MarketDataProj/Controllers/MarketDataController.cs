using MarketData.Common.Models;
using MarketData.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketDataProj.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MarketDataController : ControllerBase
    {

        private readonly ILogger<MarketDataController> _logger;
        private readonly IMarketDataService _marketDataService;
        public MarketDataController(ILogger<MarketDataController> logger, IMarketDataService service)
        {
            _logger = logger;
            _marketDataService = service;
        }

        [HttpGet(Name = "GetProductMarketData")]
        public async Task<IEnumerable<MarketDataFutureValue>> GetProductMarketData(string productCode)
        {
            return await _marketDataService.GetMarketValuesByCodeAsync(productCode);
        }

        [HttpGet(Name = "GetProductMarketDataLatest")]
        public async Task<IEnumerable<MarketDataFutureValue>> GetProductMarketDataLatest(string productCode, DateTime updateTime)
        {
            return await _marketDataService.GetMarketValuesByCodeAndUpdateTimeAsync(productCode, updateTime);
        }
    }
}