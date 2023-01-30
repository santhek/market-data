using MarketData.Common.Models;
using MarketData.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Service
{
    public class MarketDataService : IMarketDataService
    {
        private readonly ILogger<MarketDataService> _logger;
        private readonly IDbMarketRepository _repository;

        public MarketDataService(ILogger<MarketDataService> logger, IDbMarketRepository repository)
        {
            _logger = logger;
            _repository = repository;
            Jobs = Jobs ?? new ConcurrentQueue<MarketDataContributionRequest>();
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(1);

            var timer = new System.Threading.Timer((e) =>
            {
                DeQueue();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private async void DeQueue()
        {
            if(Jobs.Count> 0)
            {
                MarketDataContributionRequest request = null;
                Jobs.TryDequeue(out request);
                if(request != null )
                {
                    if(request.Data  == null)
                    {
                        _logger.LogError($"Data not supplied for request {request.Id}");
                        request.JobStatus = MarketDataJobStatus.Error; return;
                    }
                    try
                    {
                        await _repository.BulkUploadMarket(request.Id, request.Data, request.Data.Values);
                    }
                    catch (Exception ex) {
                        _logger.LogCritical(ex, $"Bulk upload failed");
                        try
                        {
                            await _repository.SingleUploadMarket(request.Id, request.Data);
                        }
                        catch(Exception ex1)
                        {
                            throw;
                        }
                    }
                    
                }
            }
        }

        public ConcurrentQueue<MarketDataContributionRequest> Jobs { get ; set ; }

        public async Task<MarketDataContributionRequest> AddToQueue(MarketDataContributionRequest job)
        {
            job.JobStatus = MarketDataJobStatus.Pending;
            Jobs.Enqueue(job);
            return await _repository.GetOrCreateMarketDataJob(job.Id, job);
        }

        public Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCodeAsync(string productCode)
        {
            return _repository.GetMarketValuesByCode(productCode);
        }

        public Task<IEnumerable<MarketDataFutureValue>> GetMarketValuesByCodeAndUpdateTimeAsync(string productCode, DateTime updateTime)
        {
            return _repository.GetMarketValuesByCodeAndUpdateTime(productCode, updateTime);
        }

        public async Task<MarketDataContributionRequest> GetStatusOrCreateJobAsync(Guid jobId, MarketDataContributionRequest job = null)
        {
            job = job ?? new MarketDataContributionRequest();
            return await _repository.GetOrCreateMarketDataJob(jobId, job);
        }
    }
}
