Printer<string> printText = Output.Show;
printText("done");

delegate void Printer<T>(T value);

static class Output
{
    public static void Show(int number)
    {
        Console.WriteLine("Show(int)");
    }

    public static void Show(string text)
    {
        Console.WriteLine("Show(string)");
    }
}
