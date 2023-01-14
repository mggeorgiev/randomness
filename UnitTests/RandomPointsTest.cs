using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using rondomness;
using System.IO;
using System.Threading.Tasks;
using Xunit;
//using Moq;

namespace UnitTests
{
    public class GenerateRandomPointsTests
    {
        private readonly ILogger<GenerateRandomPoints> _logger;
        private readonly GenerateRandomPoints _generateRandomPoints;

        public GenerateRandomPointsTests()
        {
            _logger = new LoggerFactory().CreateLogger<GenerateRandomPoints>();
            _generateRandomPoints = new GenerateRandomPoints(_logger);
        }

        [Fact]
        public async Task Run_WithValidPoints_ReturnsOkObjectResult()
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.QueryString = new QueryString("?points=10");
            var result = await _generateRandomPoints.Run(request);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Run_WithInvalidPoints_ReturnsBadRequestObjectResult()
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>()
                {
                    { "flips", "101" }
        })
            };

            //{ "groups", "111" }
            var result = await _generateRandomPoints.Run(request);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot return more than 100 coin flips.", (result as BadRequestObjectResult).Value);
        }
    }
}