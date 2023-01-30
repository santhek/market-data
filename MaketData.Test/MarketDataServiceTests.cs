using Amazon.Runtime.Internal.Util;
using MarketData.Common.Models;
using MarketData.DAL;
using MarketData.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace MaketData.Test
{
    [TestClass]
    public class MarketDataServiceTests
    {
        private IMarketDataService _marketDataService;
        private readonly Mock<ILogger<MarketDataService>> _logger;
        private readonly Mock<IDbMarketRepository> _repository;

        MarketDataServiceTests()
        {
            _logger = new Mock<ILogger<MarketDataService>>();
            _repository= new Mock<IDbMarketRepository>();   
        }

        [TestInitialize] public void Initialize()
        {
        }

        [TestMethod]
        public void TestMarketDataJobRequest()
        {
            _repository.Setup(i => i.GetOrCreateMarketDataJob(It.IsAny<Guid>(), It.IsNotNull<MarketDataContributionRequest>()))
                .Returns((Guid a, MarketDataContributionRequest m) => m);
            _marketDataService = new MarketDataService(_logger.Object, _repository.Object);
            var request = new MarketDataContributionRequest();
            request.JobStatus = MarketDataJobStatus.Submitted;
            _marketDataService.GetStatusOrCreateJobAsync(request.Id, request);
            Assert.AreEqual(MarketDataJobStatus.Submitted, request.JobStatus);

            request.JobStatus = MarketDataJobStatus.Started;
            _marketDataService.GetStatusOrCreateJobAsync(request.Id, request);
            Assert.AreEqual(MarketDataJobStatus.Started, request.JobStatus);

            request.JobStatus = MarketDataJobStatus.Success;
            _marketDataService.GetStatusOrCreateJobAsync(request.Id, request);
            Assert.AreEqual(MarketDataJobStatus.Success, request.JobStatus);

            request.JobStatus = MarketDataJobStatus.Error;
            _marketDataService.GetStatusOrCreateJobAsync(request.Id, request);
            Assert.AreEqual(MarketDataJobStatus.Error, request.JobStatus);
        }
    }
}