int failures = 4;
string level = failures switch
{
    < 3 => "low",
    >= 3 and < 5 => "medium",
};
Console.WriteLine(level);
