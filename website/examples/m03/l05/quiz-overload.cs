Console.WriteLine(Pick.Which(2));
Console.WriteLine(Pick.Which(2.0));

static class Pick
{
    public static string Which(long value) => "long";

    public static string Which(double value) => "double";
}
