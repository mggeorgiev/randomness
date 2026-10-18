using System;
using System.Collections.Generic;
using points;
using Xunit;
using Microsoft.Extensions.Logging;

namespace UnitTests
{
    public class GenerateRandomPointsTests
    {
        private readonly GenerateRandomPoints _generateRandomPoints;

        private readonly Microsoft.Extensions.Logging.ILogger<GenerateRandomPoints> _logger;

        public GenerateRandomPointsTests()
        {
            _logger = new Microsoft.Extensions.Logging.LoggerFactory().CreateLogger<GenerateRandomPoints>();
            _generateRandomPoints = new GenerateRandomPoints(_logger);
        }

        [Fact]
        public void GeneratePoints_WithValidCount_ReturnsCorrectNumber()
        {
            // Arrange
            int count = 10;

            // Act
            var result = _generateRandomPoints.GeneratePoints(count);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(count, result.Count);
        }

        [Fact]
        public void GeneratePoints_ValuesAreInRange()
        {
            // Arrange
            int count = 100;

            // Act
            var result = _generateRandomPoints.GeneratePoints(count);

            // Assert
            Assert.All(result, p => Assert.InRange(p.x, 0.0, 1.0));
            Assert.All(result, p => Assert.InRange(p.y, 0.0, 1.0));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1001)]
        public void GeneratePoints_InvalidCount_Throws(int invalidCount)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _generateRandomPoints.GeneratePoints(invalidCount));
        }
    }
}
