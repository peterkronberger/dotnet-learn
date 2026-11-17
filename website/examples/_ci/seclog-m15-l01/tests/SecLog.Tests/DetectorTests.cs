using SecLog.Model;
using SecLog.Tests.Doubles;
using Xunit;

namespace SecLog.Tests;

public class DetectorTests
{
    private static Incident NewIncident()
    {
        return new Incident("bob") { Source = "test" };
    }

    [Fact]
    public void CountMatches_TwoMatchingOneNot_ReturnsTwo()
    {
        var detector = new Detector(
        [
            new StubRule(true),
            new StubRule(false),
            new StubRule(true),
        ]);

        int matches = detector.CountMatches(NewIncident());

        Assert.Equal(2, matches);
    }

    [Fact]
    public void Check_NoRuleMatches_RaisesNoEvent()
    {
        var monitor = new IncidentMonitor(
            new Detector([new StubRule(false)]));
        var spy = new IncidentSpy();
        monitor.IncidentFound += spy.Handle;

        monitor.Check([NewIncident(), NewIncident()]);

        Assert.Empty(spy.Calls);
    }

    [Fact]
    public void Check_RuleMatches_RaisesOneEventPerIncident()
    {
        var monitor = new IncidentMonitor(
            new Detector([new StubRule(true), new StubRule(true)]));
        var spy = new IncidentSpy();
        monitor.IncidentFound += spy.Handle;

        monitor.Check([NewIncident(), NewIncident()]);

        Assert.Equal(2, spy.Calls.Count);
        Assert.All(spy.Calls, call => Assert.Equal(2, call.Matches));
    }
}
