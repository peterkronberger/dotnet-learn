int level = 2;
string name = level switch
{
    1 => "low",
    2 => "medium",
    3 => "high",
    _ => "unknown",
};
Console.WriteLine(name);
