Console.WriteLine(Load("12"));
Console.WriteLine(Load("x"));

static int Load(string text)
{
    try
    {
        int value = int.Parse(text);
        Console.WriteLine("parsed");
        return value;
    }
    catch (FormatException)
    {
        Console.WriteLine("bad text");
        return -1;
    }
    finally
    {
        Console.WriteLine("finally");
    }
}
