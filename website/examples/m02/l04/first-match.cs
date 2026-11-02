int failed = 7;
string severity = failed switch
{
    >= 5 => "high",
    >= 3 => "medium",
    _ => "low",
};
Console.WriteLine(severity);
