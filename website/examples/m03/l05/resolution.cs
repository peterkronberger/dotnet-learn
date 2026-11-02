long total = 40;

Console.WriteLine(Display.Show(3));
Console.WriteLine(Display.Show(total));
Console.WriteLine(Display.Show(3.5));

static class Display
{
    public static string Show(int value) => $"int {value}";

    public static string Show(double value) => $"double {value}";
}
