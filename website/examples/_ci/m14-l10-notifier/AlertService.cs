namespace Alerts;

public class AlertService
{
    private readonly INotifier notifier;

    public AlertService(INotifier notifier)
    {
        this.notifier = notifier;
    }

    // Sends one message if the failures reach the threshold
    public bool Alert(string user, int failures, int threshold)
    {
        if (failures < threshold)
        {
            return false;
        }
        return notifier.Send($"{user}: {failures} failed logins");
    }
}
