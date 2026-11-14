foreach (int n in new[] { 2, 6, 10 })
{
    string result = n switch
    {
        var x when x > 5 => "big",
        > 1 => "medium",
        _ => "small",
    };
    Console.WriteLine(result);
}
