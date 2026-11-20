using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class LogParserTests
{
    [Fact]
    public void ParseWithPatterns_ValidLine_ReturnsEvent()
    {
        // Arrange
        string line = "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7";

        // Act
        SecurityEvent? result = LogParser.ParseWithPatterns(line);

        // Assert
        var expected = new SecurityEvent(
            new DateTimeOffset(2026, 3, 2, 8, 1, 40, TimeSpan.Zero),
            EventType.LoginFailed,
            "bob");
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("login-success", EventType.LoginSuccess)]
    [InlineData("login-failed", EventType.LoginFailed)]
    [InlineData("logout", EventType.Logout)]
    public void ParseEventType_KnownText_ReturnsType(
        string text, EventType expected)
    {
        EventType result = LogParser.ParseEventType(text);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not a log line")]
    [InlineData("2026-03-02T08:01:40Z login-denied user=bob ip=203.0.113.7")]
    public void ParseWithPatterns_InvalidLine_ReturnsNull(string line)
    {
        SecurityEvent? result = LogParser.ParseWithPatterns(line);

        Assert.Null(result);
    }

    [Fact]
    public void Parse_InvalidLine_ReturnsLineError()
    {
        ParseResult result = LogParser.Parse("not a log line");

        Assert.True(result is LineError);
    }
}
