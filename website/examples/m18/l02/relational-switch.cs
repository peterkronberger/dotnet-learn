int[] counts = [-1, 0, 2, 3, 5];
foreach (int count in counts)
{
    Console.WriteLine($"{count}: {Rate(count)}");
}

// Rate a number of failures (the thresholds are made up)
static string Rate(int failures)
{
    return failures switch
    {
        0 => "None",
        < 3 => "Low",
        < 5 => "Medium",
        _ => "High",
    };
}
