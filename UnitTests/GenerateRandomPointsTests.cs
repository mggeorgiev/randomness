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
using System.Text;
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
        public async Task Run_WhenCalledWithValidPoints_ReturnsOkObjectResult()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            request.QueryString = new QueryString("?points=10");

            // Act
            var result = await _generateRandomPoints.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var jsonResponse = okResult.Value as string;
            Assert.NotNull(jsonResponse);

            var pointsArray = JArray.Parse(jsonResponse);
            Assert.Equal(10, pointsArray.Count);
        }

        [Fact]
        public async Task Run_WhenCalledWithInvalidPoints_ReturnsBadRequest()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>()
                {
                    { "points", "0" } // Invalid, as it's more than the allowed limit (1000)
                })
            };

            // Act
            var result = await _generateRandomPoints.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Cannot return less then 1 and more than 1000 points", badRequestResult.Value);
        }

        [Fact]
        public async Task Run_WhenCalledWithoutPoints_ReturnsBadRequest()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>()
                {
                    //{ "points", "" } // Invalid, as it's more than the allowed limit (1000)
                })
            };

            // Act
            var result = await _generateRandomPoints.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Points parameter that scpecifies the number of points is requried. Cannot return less then 1 and more than 1000 points", badRequestResult.Value);
        }

        [Fact]
        public async Task Run_WhenCalledWithEmptyPoints_ReturnsBadRequest()
        {
            // Arrange
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(new Dictionary<string, StringValues>()
                {
                    { "points", "" } // Invalid, as it's more than the allowed limit (1000)
                })
            };

            // Act
            var result = await _generateRandomPoints.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Points parameter that scpecifies the number of points is requried. Cannot return less then 1 and more than 1000 points", badRequestResult.Value);
        }
    }
}