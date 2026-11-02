int failed = 7;
string severity = failed switch
{
    0 => "none",
    < 3 => "low",
    < 5 => "medium",
};
Console.WriteLine(severity);
