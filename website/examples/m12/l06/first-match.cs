// The first arm that matches wins
foreach (int failures in new[] { 1, 3, 4, 7 })
{
    string level = failures switch
    {
        < 3 => "low",
        < 5 => "medium",
        _ => "high",
    };
    Console.WriteLine($"{failures}: {level}");
}

// The same arms with guards in another order: no compiler error
int count = 1;
string other = count switch
{
    var n when n < 5 => "medium",
    var n when n < 3 => "low",
    _ => "high",
};
Console.WriteLine(other);
