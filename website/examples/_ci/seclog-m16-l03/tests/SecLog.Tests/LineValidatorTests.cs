using Microsoft.Extensions.Logging.Abstractions;
using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class LineValidatorTests
{
    [Fact]
    public void TryParse_ValidLine_ReturnsEvent()
    {
        var validator = new LineValidator(NullLogger<LineValidator>.Instance);

        LogEvent? result = validator.TryParse(
            "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10", 1);

        Assert.NotNull(result);
    }

    [Fact]
    public void TryParse_BrokenLine_ReturnsNull()
    {
        var validator = new LineValidator(NullLogger<LineValidator>.Instance);

        LogEvent? result = validator.TryParse("broken line", 2);

        Assert.Null(result);
    }
}
