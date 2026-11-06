Printer<int> printNumber = Output.Show;
Printer<string> printText = Output.Show;
printNumber(42);
printText("done");

delegate void Printer<T>(T value);

static class Output
{
    public static void Show(int number)
    {
        Console.WriteLine($"number {number}");
    }

    public static void Show(string text)
    {
        Console.WriteLine($"text {text}");
    }
}
