using coins;
using Xunit;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    public class GenerateRandomCoinSequenceTest
    {
        private readonly GenerateRandomCoinSequence _generateRandomCoinSequence;

        public GenerateRandomCoinSequenceTest()
        {
            _generateRandomCoinSequence = new GenerateRandomCoinSequence();
        }

        [Fact]
        public void GenerateSequence_WithValidInputs_ReturnsCorrectStructure()
        {
            // Arrange
            int flips = 10;
            int groups = 16;

            // Act
            var result = _generateRandomCoinSequence.GenerateSequence(flips, groups);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(groups, result.NumberOfGroups);
            Assert.Equal(flips, result.FlipsPerGroup);
            Assert.Equal(groups, result.Data.Count);
            Assert.All(result.Data, group => Assert.Equal(flips, group.Count));
        }

        [Fact]
        public void GenerateSequence_Defaults_AreValid()
        {
            // Use defaults defined in the function: 16 flips, 8 groups
            var result = _generateRandomCoinSequence.GenerateSequence(16, 8);
            Assert.NotNull(result);
            Assert.Equal(8, result.NumberOfGroups);
            Assert.Equal(16, result.FlipsPerGroup);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 0)]
        [InlineData(101, 10)]
        [InlineData(10, 101)]
        public void GenerateSequence_InvalidInputs_Throws(int flips, int groups)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _generateRandomCoinSequence.GenerateSequence(flips, groups));
        }
    }

    public class CoinFlips
    {
        public int NumberOfGroups { get; set; }
        public int FlipsPerGroup { get; set; }
        public List<List<string>> Data { get; set; }
    }
}
