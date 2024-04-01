using coins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rondomness;
using System.IO;
using System.Threading.Tasks;
using Xunit;
//using Moq;

namespace UnitTests
{
    public class GenerateRandomCoinSequenceTest
    {
        private readonly ILogger<GenerateRandomCoinSequence> _logger;
        private readonly GenerateRandomCoinSequence _generateRandomCoinSequence;

        public GenerateRandomCoinSequenceTest()
        {
            _logger = new LoggerFactory().CreateLogger<GenerateRandomCoinSequence>();
            _generateRandomCoinSequence = new GenerateRandomCoinSequence(_logger);
        }


        [Fact]
        public async Task Run_WhenCalledWithValidFlips_ReturnsOkObjectResult()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.QueryString = new QueryString("?flips=10&groups=16");

            // Act
            var result = await _generateRandomCoinSequence.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var jsonResponse = okResult.Value as string;
            Assert.NotNull(jsonResponse);

            var coinsSequence = JsonConvert.DeserializeObject<CoinFlips>(jsonResponse);
            Assert.NotNull(coinsSequence);
            Assert.Equal(16, coinsSequence.NumberOfGroups);
            Assert.Equal(10, coinsSequence.FlipsPerGroup);
            Assert.Equal(coinsSequence.NumberOfGroups, coinsSequence.Data.Count);
        }

        [Fact]
        public async Task Run_WhenCalledWithDefault_ReturnsOkObjectResult()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.QueryString = new QueryString("");

            // Act
            var result = await _generateRandomCoinSequence.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var jsonResponse = okResult.Value as string;
            Assert.NotNull(jsonResponse);

            var coinsSequence = JsonConvert.DeserializeObject<CoinFlips>(jsonResponse);
            Assert.NotNull(coinsSequence);
            Assert.Equal(8, coinsSequence.NumberOfGroups);
            Assert.Equal(16, coinsSequence.FlipsPerGroup);
            Assert.Equal(coinsSequence.NumberOfGroups, coinsSequence.Data.Count);
        }

        [Fact]
        public async Task Run_WhenCalledWithInvalidPoints_ReturnsBadRequest()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>()
                {
                    { "flips", "101" }, // Invalid, as it's more than the allowed limit (100)
                    { "groups", "10" }
                })
            };

            // Act
            var result = await _generateRandomCoinSequence.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot return more than 100 coin flips.", badRequestResult.Value);

            // Arrange
            request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>()
                {
                    { "flips", "10" }, 
                    { "groups", "101" }// Invalid, as it's more than the allowed limit (100)
                })
            };

            // Act
            result = await _generateRandomCoinSequence.Run(request);

            // Assert
            badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot return less then 1 and more than 100 groups of flips.", badRequestResult.Value);
        }


    }
    public class CoinFlips
    {
        public int NumberOfGroups { get; set; }
        public int FlipsPerGroup { get; set; }
        public List<List<string>> Data { get; set; }
    }
}