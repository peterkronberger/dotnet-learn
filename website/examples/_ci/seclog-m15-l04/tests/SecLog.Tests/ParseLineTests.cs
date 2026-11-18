using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class ParseLineTests
{
    [Fact]
    public void ParseLine_ValidLine_ReturnsEvent()
    {
        string line = "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7";

        LogEvent result = LogParser.ParseLine(line, 2);

        var expected = new LogEvent(
            new DateTimeOffset(2026, 3, 2, 8, 1, 40, TimeSpan.Zero),
            "login-failed",
            "bob");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseLine_WrongNumberOfParts_ThrowsWithLineNumber()
    {
        var exception = Assert.Throws<InvalidLogLineException>(
            () => LogParser.ParseLine("broken line", 3));

        Assert.Equal(3, exception.LineNumber);
        Assert.Equal("expected 4 parts, found 2", exception.Message);
    }

    [Fact]
    public void ParseLine_BadTimestamp_KeepsTheCauseAsInnerException()
    {
        string line = "yesterday login-failed user=bob ip=203.0.113.7";

        var exception = Assert.Throws<InvalidLogLineException>(
            () => LogParser.ParseLine(line, 1));

        Assert.Equal("invalid timestamp", exception.Message);
        Assert.IsType<FormatException>(exception.InnerException);
    }

    [Fact]
    public void ParseLine_UnknownEventType_NamesTheType()
    {
        string line = "2026-03-02T08:01:40Z login-denied user=bob ip=203.0.113.7";

        var exception = Assert.Throws<InvalidLogLineException>(
            () => LogParser.ParseLine(line, 1));

        Assert.Contains("login-denied", exception.Message);
    }
}
