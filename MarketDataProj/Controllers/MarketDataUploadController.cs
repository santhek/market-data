using MarketData.Common.Models;
using MarketData.Service;
using MarketDataProj.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace MarketDataProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketDataUploadController : ControllerBase
    {        

        private readonly ILogger<MarketDataUploadController> _logger;
        private readonly IMarketDataService _marketDataService;
        public MarketDataUploadController(ILogger<MarketDataUploadController> logger, IMarketDataService service)
        {
            _logger = logger;
            _marketDataService = service;
        }

        /// <summary>
        /// Gets the status of an upload
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetMarketDataJobDetail")]
        public async Task<IActionResult> GetJobDetail(Guid jobId)
        {
            var response = new MarketRequestOutput();
            if(jobId == Guid.Empty)
            {
                response.Error = "Guid cant be empty";
                return BadRequest(response);
            }
            response.Request =  await _marketDataService.GetStatusOrCreateJobAsync(jobId);

            return Ok(response);
        }

        /// <summary>
        /// Submits Market data upload request
        /// </summary>
        /// <param name="publisher">Source of the data e.g. CME</param>
        /// <param name="productCode">Exchange code e.g. CL</param>
        /// <param name="unit">Units of the traded product. e.g. 1000 Barrels</param>
        /// <param name="values"> Value details</param>
        /// <returns></returns>
        [HttpPost(Name = "SubmitMarketData")]        
        public async Task<IActionResult> SubmitMarketData(string publisher, string productCode, string unit, IList<MarketDataFutureValue> values)
        {
            MarketDataContributionRequest result;
            if(string.IsNullOrWhiteSpace(publisher))
            {
                return BadRequest($"{nameof(publisher)} cant be null or empty");
            }
            else if (string.IsNullOrWhiteSpace(productCode))
            {
                return BadRequest($"{nameof(productCode)} cant be null or empty");
            }
            else if (string.IsNullOrWhiteSpace(unit))
            {
                return BadRequest($"{nameof(unit)} cant be null or empty");
            }
            else if (values == null || values.Count == 0)
            {
                return BadRequest($"{nameof(values)} cant be null or empty");
            }

            try
            {
                var marketDataJob = new MarketDataContributionRequest() { Data = new MarketDataFutures(publisher, productCode, unit, values) };
                _ = await _marketDataService.GetStatusOrCreateJobAsync(marketDataJob.Id, marketDataJob);
                result = await _marketDataService.AddToQueue(marketDataJob);
                return Ok(result);  
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Market data Upload error");
                return BadRequest($"Market data Upload error");
            }
        }
        
        

    }
}