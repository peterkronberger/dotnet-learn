string[] lines = ["failures=4", "failures=x", "failures"];

foreach (string line in lines)
{
    Console.WriteLine($"General catch: {CountGeneral(line)}");
}

foreach (string line in lines)
{
    try
    {
        Console.WriteLine($"Specific catch: {CountSpecific(line)}");
    }
    catch (IndexOutOfRangeException ex)
    {
        Console.WriteLine($"Bug found: {ex.GetType().Name}");
    }
}

int CountGeneral(string line)
{
    try
    {
        return int.Parse(line.Split('=')[1]);
    }
    catch (Exception)
    {
        return 0;
    }
}

int CountSpecific(string line)
{
    try
    {
        return int.Parse(line.Split('=')[1]);
    }
    catch (FormatException)
    {
        return 0;
    }
}
