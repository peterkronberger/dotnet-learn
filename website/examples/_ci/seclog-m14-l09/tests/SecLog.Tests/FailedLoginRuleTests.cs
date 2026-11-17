using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class FailedLoginRuleTests
{
    private static LogEvent Failure(string user, int second)
    {
        var time = new DateTimeOffset(2026, 3, 2, 8, 0, second, TimeSpan.Zero);
        return new LogEvent(time, "login-failed", user);
    }

    private static Incident IncidentWithFailures(string user, int count)
    {
        var incident = new Incident(user) { Source = "test" };
        for (int i = 0; i < count; i++)
        {
            incident.Add(Failure(user, i));
        }
        return incident;
    }

    [Fact]
    public void IsMatch_FailuresEqualThreshold_ReturnsTrue()
    {
        // Arrange
        var rule = new FailedLoginRule(5);
        var incident = IncidentWithFailures("bob", 5);

        // Act
        bool result = rule.IsMatch(incident);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsMatch_FailuresBelowThreshold_ReturnsFalse()
    {
        var rule = new FailedLoginRule(5);
        var incident = IncidentWithFailures("bob", 4);

        bool result = rule.IsMatch(incident);

        Assert.False(result);
    }

    [Fact]
    public void Describe_ContainsThreshold()
    {
        var rule = new FailedLoginRule(5);

        string text = rule.Describe();

        Assert.Equal("Rule: failed logins (threshold 5)", text);
    }

    [Fact]
    public void Add_OtherUser_IsIgnored()
    {
        var incident = new Incident("bob") { Source = "test" };

        bool added = incident.Add(Failure("alice", 1));

        Assert.False(added);
        Assert.Equal(0, incident.FailureCount);
    }
}
