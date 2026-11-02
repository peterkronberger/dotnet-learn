Console.WriteLine(Display.Show(7));
Console.WriteLine(Display.Show(2.5));
Console.WriteLine(Display.Show("ok"));

static class Display
{
    public static string Show(int value) => $"int {value}";

    public static string Show(double value) => $"double {value}";

    public static string Show(string value) => $"string {value}";
}
