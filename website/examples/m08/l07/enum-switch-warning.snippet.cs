string Label(Severity severity) => severity switch
{
    Severity.None => "none",
    Severity.Low => "low",
    Severity.Medium => "medium",
    Severity.High => "high",
};

enum Severity { None, Low, Medium, High }
