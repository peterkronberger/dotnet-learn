Console.WriteLine(Display.Show(7));
Console.WriteLine(Display.Show(7.5));

static class Display
{
    public static string Show(double value) => $"double {value}";
}
