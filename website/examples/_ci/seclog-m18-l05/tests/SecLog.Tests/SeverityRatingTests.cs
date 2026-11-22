using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class SeverityRatingTests
{
    [Theory]
    [InlineData(0, Severity.None)]
    [InlineData(1, Severity.Low)]
    [InlineData(2, Severity.Low)]
    [InlineData(3, Severity.Medium)]
    [InlineData(4, Severity.Medium)]
    [InlineData(5, Severity.High)]
    [InlineData(1000, Severity.High)]
    public void For_FailureCount_ReturnsExpected(
        int failureCount, Severity expected)
    {
        Severity result = SeverityRating.For(failureCount);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void For_NegativeCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SeverityRating.For(-1));
    }
}
