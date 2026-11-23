// Describe an alert: the compiler checks that all subtypes are handled
Alert alert = new Warning(3);

string text = alert switch
{
    Info => "just information",
    Warning warning => $"warning after {warning.Failures} failures",
};
Console.WriteLine(text);

closed record Alert;
sealed record Info : Alert;
sealed record Warning(int Failures) : Alert;
