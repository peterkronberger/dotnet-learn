using Xunit;

namespace Alerts.Tests;

public class AlertServiceTests
{
    [Fact]
    public void Alert_BelowThreshold_SendsNothing()
    {
        var spy = new SpyNotifier();
        var service = new AlertService(spy);

        service.Alert("bob", failures: 4, threshold: 5);

        Assert.Empty(spy.Messages);
    }

    [Fact]
    public void Alert_AtThreshold_SendsOneMessage()
    {
        var spy = new SpyNotifier();
        var service = new AlertService(spy);

        service.Alert("bob", failures: 5, threshold: 5);

        Assert.Equal(["bob: 5 failed logins"], spy.Messages);
    }

    [Fact]
    public void Alert_SendFails_ReturnsFalse()
    {
        var service = new AlertService(new StubNotifier(false));

        bool result = service.Alert("bob", failures: 5, threshold: 5);

        Assert.False(result);
    }

    [Fact]
    public void Alert_SameAlertTwice_SecondIsNotDelivered()
    {
        var service = new AlertService(new InMemoryNotifier());

        bool first = service.Alert("bob", failures: 5, threshold: 5);
        bool second = service.Alert("bob", failures: 5, threshold: 5);

        Assert.True(first);
        Assert.False(second);
    }
}
