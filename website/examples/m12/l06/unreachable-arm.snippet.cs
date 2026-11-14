int failures = 4;
string level = failures switch
{
    >= 3 => "medium",
    >= 5 => "high",
    _ => "low",
};
Console.WriteLine(level);
