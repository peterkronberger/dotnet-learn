long total = 40;
Display.Show(total);

static class Display
{
    public static void Show(int value)
    {
        Console.WriteLine($"int {value}");
    }

    public static void Show(double value)
    {
        Console.WriteLine($"double {value}");
    }
}
