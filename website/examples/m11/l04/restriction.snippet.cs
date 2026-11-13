static IEnumerable<int> Read(string text)
{
    try
    {
        yield return int.Parse(text);
    }
    catch (FormatException)
    {
        Console.WriteLine("not a number");
    }
}

Console.WriteLine(string.Join(", ", Read("12")));
