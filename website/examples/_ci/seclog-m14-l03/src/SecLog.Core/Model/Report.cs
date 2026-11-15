// Format a report line: the overloads choose the number format
namespace SecLog.Model;

public static class Report
{
    public static string Line(string label, int value)
    {
        return $"{label}: {value}";
    }

    public static string Line(string label, double value)
    {
        return $"{label}: {value:F1}";
    }
}
