namespace Alarm;

public static class AlarmRule
{
    // The rule: 5 or more failures are high, 3 or more are medium
    public static string Level(int failures)
    {
        if (failures >= 5)
        {
            return "high";
        }
        if (failures > 3)
        {
            return "medium";
        }
        return "low";
    }
}
