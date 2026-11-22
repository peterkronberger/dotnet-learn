using System.ComponentModel.DataAnnotations;
using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class DetectionOptionsTests
{
    private static bool IsValid(DetectionOptions options)
    {
        return Validator.TryValidateObject(
            options, new ValidationContext(options), null, true);
    }

    [Fact]
    public void Defaults_AreValid()
    {
        Assert.True(IsValid(new DetectionOptions()));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(5, 0)]
    [InlineData(-1, -1)]
    public void NotPositiveValue_IsInvalid(int maxFailures, int windowMinutes)
    {
        var options = new DetectionOptions
        {
            MaxFailures = maxFailures,
            WindowMinutes = windowMinutes,
        };

        Assert.False(IsValid(options));
    }
}
